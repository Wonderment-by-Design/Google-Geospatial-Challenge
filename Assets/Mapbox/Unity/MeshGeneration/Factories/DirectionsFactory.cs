namespace Mapbox.Unity.MeshGeneration.Factories
{
	using UnityEngine;
	using Mapbox.Directions;
	using System.Collections.Generic;
	using System.Linq;
	using Mapbox.Unity.Map;
	using Data;
	using Modifiers;
	using Mapbox.Utils;
	using Mapbox.Unity.Utilities;
	using System.Collections;
    using System;

	public class DirectionsFactory : MonoBehaviour
	{
		[SerializeField]
		AbstractMap _map;

		[SerializeField]
		MeshModifier[] MeshModifiers;
		[SerializeField]
		Material _material;

		[SerializeField]
		Transform[] _waypoints;
		private List<Vector3> _cachedWaypoints;

        [SerializeField]
		[Range(1,10)]
		private float UpdateFrequency = 2;

		bool disabledOnce = false;

		bool updateAR = true;



		private Directions _directions;
		private int _counter;

		GameObject _directionsGO;
		private bool _recalculateNext;

        public Transform[] Waypoints { get => _waypoints; set => _waypoints = value; }

        protected virtual void Awake()
		{
			if (_map == null)
			{
				_map = FindObjectOfType<AbstractMap>();
			}
			_directions = MapboxAccess.Instance.Directions;
			//_map.OnInitialized += Query;
			//_map.OnUpdated += Query;
        }

		public void Start()
		{
			_cachedWaypoints = new List<Vector3>(Waypoints.Length);
			foreach (var item in Waypoints)
			{
				_cachedWaypoints.Add(item.position);
			}
			_recalculateNext = false;

			foreach (var modifier in MeshModifiers)
			{
				modifier.Initialize();
			}

			StartCoroutine(QueryTimer());
		}

        private void OnEnable()
        {
            if(disabledOnce == true)
			{
                //_map.OnInitialized += Query;
               // _map.OnUpdated += Query;

                _cachedWaypoints = new List<Vector3>(Waypoints.Length);
                foreach (var item in Waypoints)
                {
                    _cachedWaypoints.Add(item.position);
                }
                _recalculateNext = false;

                foreach (var modifier in MeshModifiers)
                {
                    modifier.Initialize();
                }

                StartCoroutine(QueryTimer());
            }
        }

        private void OnDisable()
        {
			StopAllCoroutines();
            _map.OnInitialized -= Query;
            _map.OnUpdated -= Query;
            disabledOnce = true;
			Destroy(_directionsGO);
        }

        protected virtual void OnDestroy()
		{
			_map.OnInitialized -= Query;
			_map.OnUpdated -= Query;
		}

		public void CreateRoute(){
			updateAR = true;
			Query();
		}

		void Query()
		{
			var count = Waypoints.Length;
			var wp = new Vector2d[count];
			for (int i = 0; i < count; i++)
			{
				wp[i] = Waypoints[i].GetGeoPosition(_map.CenterMercator, _map.WorldRelativeScale);
			}
			var _directionResource = new DirectionResource(wp, RoutingProfile.Driving);
			_directionResource.Steps = true;
			_directions.Query(_directionResource, HandleDirectionsResponse);
		}

		public IEnumerator QueryTimer()
		{
			while (true)
			{
				yield return new WaitForSeconds(UpdateFrequency);
				for (int i = 0; i < Waypoints.Length; i++)
				{
					if (Waypoints[i].position != _cachedWaypoints[i])
					{
						_recalculateNext = true;
						_cachedWaypoints[i] = Waypoints[i].position;
					}
				}

				if (_recalculateNext)
				{
					Query();
					_recalculateNext = false;
				}
			}
		}

		void HandleDirectionsResponse(DirectionsResponse response)
		{
			if (response == null || null == response.Routes || response.Routes.Count < 1)
			{
				return;
			}

			var meshData = new MeshData();
			var dat = new List<Vector3>();
			MapController.Instance.SetDistance(response.Routes[0].Distance);
			MapController.Instance.UpdateRoutePoints(response.Routes[0].Geometry.ToArray());
			if (updateAR)
			{
				MapController.Instance.UpdateARRoute();
				updateAR = false;
			}

            foreach (var point in response.Routes[0].Geometry)
			{
				dat.Add(Conversions.GeoToWorldPosition(point.x, point.y, _map.CenterMercator, _map.WorldRelativeScale).ToVector3xz());
			}
			//MapController.Instance.UpdateMapRoute(dat.ToArray());

			var feat = new VectorFeatureUnity();
			feat.Points.Add(dat);

			foreach (MeshModifier mod in MeshModifiers.Where(x => x.Active))
			{
				mod.Run(feat, meshData, _map.WorldRelativeScale);
			}

			//CreateGameObject(meshData);
		}

		GameObject CreateGameObject(MeshData data)
		{
			if (_directionsGO != null)
			{
				_directionsGO.Destroy();
			}
			_directionsGO = new GameObject("direction waypoint " + " entity");
			_directionsGO.layer = 6;//map
			var mesh = _directionsGO.AddComponent<MeshFilter>().mesh;
			mesh.subMeshCount = data.Triangles.Count;

			mesh.SetVertices(data.Vertices);
			_counter = data.Triangles.Count;
			for (int i = 0; i < _counter; i++)
			{
				var triangle = data.Triangles[i];
				mesh.SetTriangles(triangle, i);
			}

			_counter = data.UV.Count;
			for (int i = 0; i < _counter; i++)
			{
				var uv = data.UV[i];
				mesh.SetUVs(i, uv);
			}

			mesh.RecalculateNormals();
			_directionsGO.AddComponent<MeshRenderer>().material = _material;
			return _directionsGO;
		}
	}

}
