using System;
using UnityEngine;

public class PlayerStatus : MonoBehaviour
{
   public bool isHunter;
   public SkinnedMeshRenderer skinRenderer;
   private Material material;

   private void Awake()
   {
      material = skinRenderer.material;
   }

   private void Start()
   {
      GameManager.Instance.players.Add(this);
   }
   
   private void Update()
   {
      material.color = isHunter ? Color.magenta : Color.blue;
   }
}
