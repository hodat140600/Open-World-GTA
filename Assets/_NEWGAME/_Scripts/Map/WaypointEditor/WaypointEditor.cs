using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Extensions;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;

namespace _GAME._Scripts
{
    [ExecuteInEditMode]
    public class WaypointEditor : MonoBehaviour
    {
        private enum DrawerMode
        {
            Disabled,
            Draw,
            Erase
        }

        [SerializeField]
        [ReadOnly]
        private int waypointCounter;

        [PropertyOrder(100)]
        [FoldoutGroup("Configurable")]
        [SerializeField]
        private WaypointOnEditor waypointOnEditorPrefab;

        [PropertyOrder(100)]
        [FoldoutGroup("Configurable")]
        [SerializeField]
        private Transform waypointOnEditorHolder;

        [PropertyOrder(100)]
        [FoldoutGroup("Configurable")]
        [SerializeField]
        private string waypointNamePrefix = "w";

        [PropertyOrder(100)]
        [FoldoutGroup("Configurable")]
        [SerializeField]
        [OnValueChanged(nameof(Load))]
        private MapSettings mapSettings;

        public MapSettings MapSettings => mapSettings;

        [PropertyOrder(101)]
        [FoldoutGroup("Danger Zone")]
        [SerializeField]
        [GUIColor(.8f, .8f, .8f)]
        public List<WaypointOnEditor> waypointOnEditors;

        [PropertyOrder(101)]
        [FoldoutGroup("Danger Zone")]
        [Button("Reload From Editor", ButtonSizes.Large)]
        [GUIColor(0.31f, 0.55f, 0.93f, 1f)]
        public void ReloadMapSettings()
        {
            waypointOnEditors            = new List<WaypointOnEditor>();
            mapSettings.Entity.Waypoints = new List<Waypoint>();

            foreach (Transform waypointOnEditorTransform in waypointOnEditorHolder)
            {
                WaypointOnEditor waypointOnEditor = waypointOnEditorTransform.GetComponent<WaypointOnEditor>();
                waypointOnEditors.Add(waypointOnEditor);
                mapSettings.Entity.Waypoints.Add(waypointOnEditor.ToWaypoint);
            }

            mapSettings.Entity.UpdateWaypointDictionary();
        }

        [PropertyOrder(101)]
        [FoldoutGroup("Danger Zone")]
        [Button("Reset All", ButtonSizes.Large)]
        [GUIColor(1, .2f, .2f)]
        public void ResetAll()
        {
            RemoveAllWaypoints();
            SaveWaypoints();
        }

        private IEnumerator routine;

        private DrawerMode WAYPOINT_DRAWER_MODE;

        [OnValueChanged(nameof(ResetSelectedWaypoint))]
        private DrawerMode ROUTE_DRAWER_MODE;

        private DrawerMode NPC_ROUTE_DRAWER_MODE;

        private bool SHOW_GRAPH;

        private GUIStyle waypointNameLabelStyle;
        private GUIStyle startEndRouteLabelStyle;

        private const string StartString = "Start";
        private const string EndString   = "End";

        private readonly Vector3 startEndTextOffset = 0.5f * Vector3.one;
        // private Dictionary<int, Waypoint> IdToWaypoint => mapSettings.Entity.IdToWaypoint;

        [HorizontalGroup("Drawer")]
        [BoxGroup("Drawer/Waypoint")]
        [HideLabel]
        [Button("DRAWING ('A')", ButtonSizes.Large)]
        [GUIColor(0, 1, 0)]
        [ShowIf("@this.WAYPOINT_DRAWER_MODE == DrawerMode.Draw", false)]
        private void WaypointDraw()
        {
            WAYPOINT_DRAWER_MODE = DrawerMode.Erase;
        }

        [HorizontalGroup("Drawer")]
        [BoxGroup("Drawer/Waypoint")]
        [HideLabel]
        [Button("ERASING ('A')", ButtonSizes.Large)]
        [GUIColor(1f, .4f, .4f)]
        [ShowIf("@this.WAYPOINT_DRAWER_MODE == DrawerMode.Erase", false)]
        private void WaypointErase()
        {
            WAYPOINT_DRAWER_MODE = DrawerMode.Disabled;
        }

        [HorizontalGroup("Drawer")]
        [BoxGroup("Drawer/Waypoint")]
        [HideLabel]
        [Button("DISABLED ('A')", ButtonSizes.Large)]
        [ShowIf("@this.WAYPOINT_DRAWER_MODE == DrawerMode.Disabled", false)]
        private void WaypointDisabled()
        {
            WAYPOINT_DRAWER_MODE = DrawerMode.Draw;
        }

        [HorizontalGroup("Drawer")]
        [BoxGroup("Drawer/Route")]
        [HideLabel]
        [Button("DRAWING ('S')", ButtonSizes.Large)]
        [GUIColor(0, 1, 0)]
        [ShowIf("@this.ROUTE_DRAWER_MODE == DrawerMode.Draw", false)]
        private void RouteDraw()
        {
            ROUTE_DRAWER_MODE = DrawerMode.Erase;
        }

        [HorizontalGroup("Drawer")]
        [BoxGroup("Drawer/Route")]
        [HideLabel]
        [Button("ERASING ('S')", ButtonSizes.Large)]
        [GUIColor(1f, .4f, .4f)]
        [ShowIf("@this.ROUTE_DRAWER_MODE == DrawerMode.Erase", false)]
        private void RouteErase()
        {
            ROUTE_DRAWER_MODE = DrawerMode.Disabled;
        }

        [HorizontalGroup("Drawer")]
        [BoxGroup("Drawer/Route")]
        [HideLabel]
        [Button("DISABLED ('S')", ButtonSizes.Large)]
        [ShowIf("@this.ROUTE_DRAWER_MODE == DrawerMode.Disabled", false)]
        private void RouteDisabled()
        {
            ROUTE_DRAWER_MODE = DrawerMode.Draw;
        }

        [PropertyOrder(1)]
        [SerializeField]
        [HorizontalGroup("SelectedWaypoint")]
        [PropertySpace(spaceBefore: 3f)]
        private WaypointOnEditor selectedWaypoint;

        [PropertyOrder(1)]
        [Button("REFRESH ('D')", ButtonSizes.Medium)]
        [HorizontalGroup("SelectedWaypoint")]
        [GUIColor(0, .6f, 1)]
        private void ResetSelectedWaypoint()
        {
            selectedWaypoint = null;
        }

        [PropertyOrder(2)]
        [BoxGroup("NPC Route")]
        [SerializeField]
        [HorizontalGroup("NPC Route/Group", .3f)]
        [HideLabel]
        [OnValueChanged(nameof(DisableDrawerIfNull))]
        [PropertySpace(spaceBefore: 7f)]
        private RouteSettings routeSettings;

        private void DisableDrawerIfNull()
        {
            if (routeSettings == null)
                NPC_ROUTE_DRAWER_MODE = DrawerMode.Disabled;
        }

        [PropertyOrder(2)]
        [BoxGroup("NPC Route")]
        [HorizontalGroup("NPC Route/Group")]
        [Button("DRAWING ('C')", ButtonSizes.Large)]
        [GUIColor(0, 1, 0)]
        [ShowIf("@this.NPC_ROUTE_DRAWER_MODE == DrawerMode.Draw", false)]
        private void NpcRouteDraw()
        {
            NPC_ROUTE_DRAWER_MODE = DrawerMode.Erase;
        }

        [PropertyOrder(2)]
        [BoxGroup("NPC Route")]
        [HorizontalGroup("NPC Route/Group")]
        [Button("ERASING ('C')", ButtonSizes.Large)]
        [GUIColor(1f, .4f, .4f)]
        [ShowIf("@this.NPC_ROUTE_DRAWER_MODE == DrawerMode.Erase", false)]
        private void NpcRouteErase()
        {
            NPC_ROUTE_DRAWER_MODE = DrawerMode.Disabled;
        }

        [PropertyOrder(2)]
        [BoxGroup("NPC Route")]
        [HorizontalGroup("NPC Route/Group")]
        [Button("DISABLED ('C')", ButtonSizes.Large)]
        [ShowIf("@this.NPC_ROUTE_DRAWER_MODE == DrawerMode.Disabled", false)]
        private void NpcRouteDisabled()
        {
            if (routeSettings == null) return;
            NPC_ROUTE_DRAWER_MODE = DrawerMode.Draw;
        }

        public void Load()
        {
            RemoveAllWaypoints();

            foreach (var waypoint in mapSettings.Entity.Waypoints)
                CreateNewWaypointOnEditor(waypoint.Name, waypoint.Position);
        }

        private void Update()
        {
            routine?.MoveNext();
            waypointOnEditorHolder.gameObject.SetActive(!Application.isPlaying);
        }

        private void Save()
        {
            SaveWaypoints();
            BakeGraph();
        }

        #region ON_SCENE

#if UNITY_EDITOR
        private void OnEnable()
        {
            if (!Application.isEditor)
            {
                WAYPOINT_DRAWER_MODE = DrawerMode.Disabled;
                ROUTE_DRAWER_MODE    = DrawerMode.Disabled;
            }

            waypointNameLabelStyle                  = new GUIStyle();
            waypointNameLabelStyle.normal.textColor = Color.blue;

            startEndRouteLabelStyle                  = new GUIStyle();
            startEndRouteLabelStyle.normal.textColor = Color.red;
            startEndRouteLabelStyle.fontSize         = 20;
            startEndRouteLabelStyle.fontStyle        = FontStyle.Bold;

            waypointOnEditors ??= new List<WaypointOnEditor>();
            if (!mapSettings.Entity.IsWaypointsNull)
            {
                ReloadMapSettings();
            }

            SceneView.duringSceneGui += OnScene;
        }

        private void OnScene(SceneView scene)
        {
            bool notDoingAnything =
                WAYPOINT_DRAWER_MODE == DrawerMode.Disabled
                && ROUTE_DRAWER_MODE == DrawerMode.Disabled
                && NPC_ROUTE_DRAWER_MODE == DrawerMode.Disabled;

            if (notDoingAnything) return;

            Event e = Event.current;

            if (e.type == EventType.KeyDown)
            {
                HandleHotKey(e.keyCode);
                e.Use();
                return;
            }

            bool isLeftMouseClicked = e.type == EventType.MouseDown && e.button == 0;
            if (!isLeftMouseClicked) return;

            if (!HitGameObject(scene, e, out RaycastHit hit)) return;

            if (!HitWaypointOnEditor(hit, out WaypointOnEditor waypoint))
            {
                if (WAYPOINT_DRAWER_MODE == DrawerMode.Draw)
                {
                    string waypointName = waypointNamePrefix + waypointCounter;
                    waypointCounter++;
                    CreateNewWaypointOnEditor(waypointName, hit.point);
                    Save();
                    e.Use();
                }

                return;
            }

            if (selectedWaypoint != null)
            {
                if (ROUTE_DRAWER_MODE == DrawerMode.Draw)
                {
                    selectedWaypoint.AddNeighbour(waypoint);
                    waypoint.AddNeighbour(selectedWaypoint);
                    Save();
                }
                else if (ROUTE_DRAWER_MODE == DrawerMode.Erase)
                {
                    selectedWaypoint.RemoveNeighbour(waypoint);
                    waypoint.RemoveNeighbour(selectedWaypoint);
                    Save();
                }

                if (WAYPOINT_DRAWER_MODE == DrawerMode.Erase)
                {
                    DestroyWaypointOnEditor(waypoint);
                    Save();
                }

                if (NPC_ROUTE_DRAWER_MODE == DrawerMode.Draw)
                {
                    if (routeSettings.Route.waypointsIds == null)
                        routeSettings.Route.waypointsIds = new List<int>() { waypoint.ToWaypoint.Id };
                    else if (routeSettings.Route.waypointsIds.Count == 0)
                        routeSettings.Route.waypointsIds.Add(waypoint.ToWaypoint.Id);

                    else
                    {
                        routine = PathfinderUtilities.FindPath(
                            mapSettings.Entity.GetWaypointsByIds,
                            WaypointOf(routeSettings.Route.waypointsIds[^1]),
                            waypoint.ToWaypoint,
                            result =>
                            {
                                if (result == null) return;
                                List<Waypoint> exceptStart = result.GetRange(1, result.Count - 1);
                                routeSettings.Route.waypointsIds.AddRange(exceptStart.Select(w => w.Id));

                                EditorUtility.SetDirty(routeSettings);
                                AssetDatabase.SaveAssets();
                            });
                        // "PathfinderUtilities.FindPath".Log();
                    }
                }
            }

            selectedWaypoint = waypoint;

            e.Use();
        }

        private static bool HitGameObject(SceneView scene, Event e, out RaycastHit hit)
        {
            return Physics.Raycast(GetMouseRaycastOnScreen(scene, e), out hit);
        }

        private void HandleHotKey(KeyCode key)
        {
            switch (key)
            {
                case KeyCode.D:
                    ResetSelectedWaypoint();
                    break;

                case KeyCode.A:
                    WAYPOINT_DRAWER_MODE = NextDrawerMode(WAYPOINT_DRAWER_MODE);
                    break;

                case KeyCode.S:
                    ROUTE_DRAWER_MODE = NextDrawerMode(ROUTE_DRAWER_MODE);
                    break;

                case KeyCode.C:
                    NPC_ROUTE_DRAWER_MODE = NextDrawerMode(NPC_ROUTE_DRAWER_MODE);
                    break;
            }
        }

        private DrawerMode NextDrawerMode(DrawerMode current) => current switch
        {
            DrawerMode.Draw     => DrawerMode.Erase,
            DrawerMode.Erase    => DrawerMode.Disabled,
            DrawerMode.Disabled => DrawerMode.Draw,
            _                   => throw new ArgumentOutOfRangeException()
        };


        private static bool HitWaypointOnEditor(RaycastHit hit, out WaypointOnEditor waypoint)
        {
            return hit.transform.TryGetComponent(out waypoint);
        }

        private static Ray GetMouseRaycastOnScreen(SceneView scene, Event e)
        {
            Vector3 mousePos = e.mousePosition;
            float   ppp      = EditorGUIUtility.pixelsPerPoint;
            mousePos.y =  scene.camera.pixelHeight - mousePos.y * ppp;
            mousePos.x *= ppp;

            Ray ray = scene.camera.ScreenPointToRay(mousePos);
            return ray;
        }

        private Waypoint WaypointOf(int Id)
        {
            return mapSettings.Entity.GetWaypointById(Id);
        }

        private void OnDrawGizmos()
        {
            foreach (Waypoint waypoint in mapSettings.Entity.Waypoints)
            {
                Handles.Label(waypoint.Position, waypoint.Name, waypointNameLabelStyle);

                foreach (var neighbourId in waypoint.neighbourIds)
                    DrawLineBetween(
                        waypoint.Position,
                        WaypointOf(neighbourId).Position, Color.blue, 2f);
            }

            if (routeSettings == null) return;

            var npcRoute = routeSettings.Route.waypointsIds;
            if (npcRoute == null || npcRoute.Count == 0) return;

            for (int i = 0; i < npcRoute.Count - 1; i++)
            {
                var current = npcRoute[i];
                var next    = npcRoute[i + 1];
                var ratio   = ((float)i / (npcRoute.Count));

                var color = Color.Lerp(Color.yellow, Color.red, ratio);
                var width = Mathf.Lerp(15f, 7f, ratio);

                DrawLineBetween(WaypointOf(current).Position, WaypointOf(next).Position, color, width);
            }

            Handles.Label(WaypointOf(npcRoute[0]).Position + startEndTextOffset, StartString, startEndRouteLabelStyle);
            Handles.Label(WaypointOf(npcRoute[^1]).Position - startEndTextOffset, EndString, startEndRouteLabelStyle);
        }

        private void DrawLineBetween(Vector3 start, Vector3 end, Color color, float width)
        {
            Handles.DrawBezier(start, end, start, end, color, null, width);
        }
#endif

        private void CreateNewWaypointOnEditor(string waypointName, Vector3 position)
        {
            WaypointOnEditor waypoint = Instantiate(waypointOnEditorPrefab, position, Quaternion.identity, waypointOnEditorHolder);
            waypoint.name = waypointName;
            waypoint.InitWaypoint();
            waypointOnEditors.Add(waypoint);
        }

        private void DestroyWaypointOnEditor(WaypointOnEditor current)
        {
            int currentId = current.ToWaypoint.Id;
            foreach (WaypointOnEditor waypointOnEditor in waypointOnEditors)
                waypointOnEditor.ToWaypoint.neighbourIds.Remove(currentId);

            waypointOnEditors.Remove(current);
            DestroyImmediate(current.gameObject);
        }

        #endregion ON_SCENE

        private void SaveWaypoints()
        {
#if UNITY_EDITOR
            EditorUtility.SetDirty(mapSettings);
#endif

            mapSettings.Entity.Waypoints.Clear();

            mapSettings.Entity.Waypoints =
                waypointOnEditors
                    .Select(waypointOnEditor => waypointOnEditor.ToWaypoint)
                    .Distinct()
                    .ToList();

#if UNITY_EDITOR
            EditorUtility.SetDirty(mapSettings);
#endif
        }

        private void BakeGraph()
        {
            mapSettings.Entity.UpdateWaypointDictionary();
        }

        private void RemoveAllWaypoints()
        {
            waypointCounter = 0;
            waypointOnEditors.Clear();
            waypointOnEditorHolder.DestroyImmediateAllChildren();
        }
    }
}