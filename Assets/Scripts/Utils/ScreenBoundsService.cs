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
        public ScreenBoundsService() => cam = Camera.main;
        public float minX => -GetHalfWidth();

        public float maxX => GetHalfWidth();

        public float minY => -GetHalfHeight() + GetHudHeightInWorldUnits();

        public float maxY => GetHalfHeight();

        private float GetHalfWidth() => cam.orthographicSize * cam.aspect;
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
