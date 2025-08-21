using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Utils
{
    public class ScreenBoundsService : IBoundsService
    {
        Camera cam;
        private const float HUD_HEIGHT_PERCENTAGE = 0.1f;
        [SerializeField] private float targetAspect = 9f / 16f;
        public ScreenBoundsService() => cam = Camera.main;
        public float minX => -GetHalfWidth();

        public float maxX => GetHalfWidth();

        public float minY => -GetHalfHeight() + GetHudHeightInWorldUnits();

        public float maxY => GetHalfHeight();

        private float GetHalfWidth()
        {
            float halfHeight = cam.orthographicSize;
            float cameraHalfWidth = halfHeight * cam.aspect;
            float targetHalfWidth = halfHeight * targetAspect; 
            return Mathf.Min(cameraHalfWidth, targetHalfWidth); 
        }
        private float GetHalfHeight() => cam.orthographicSize;
        /// <summary>
        /// Calculate HUD height in world units
        /// </summary>
        /// <returns>HUD height in world coordinate units</returns>
        private float GetHudHeightInWorldUnits()
        {
            return GetHalfHeight() * 2 * HUD_HEIGHT_PERCENTAGE;
        }
    }
}
