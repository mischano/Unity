using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// https://catlikecoding.com/unity/tutorials/movement/custom-gravity/
public class CustomGravity : MonoBehaviour {
   public static Vector3 GetGravity(Vector3 position) {
      Vector3 up = position.normalized;
      return up * Physics.gravity.y;
   }

   public static Vector3 GetUpAxis(Vector3 position) {
      Vector3 up = position.normalized;
      return -Physics.gravity.y < 0f ? up : -up;
   }

   public static Vector3 GetGravity(Vector3 position, out Vector3 upAxis) {
      Vector3 up = position.normalized;
      upAxis = Physics.gravity.y < 0f ? up : -up;
      return up * Physics.gravity.y;
   }
}
