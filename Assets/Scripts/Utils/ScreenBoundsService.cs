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

        public ScreenBoundsService() => cam = Camera.main;
        public float minX => -GetHalfWidth();

        public float maxX => GetHalfWidth();

        public float minY => -GetHalfHeight();

        public float maxY => GetHalfHeight();

        private float GetHalfWidth() => cam.orthographicSize * cam.aspect;
        private float GetHalfHeight() => cam.orthographicSize;
    }
}
