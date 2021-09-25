using System;

namespace ZebraCorn.Rules.CodeChecks
{
    public class UnityCodeCheck : ICodeCheck
    {
        public Single CodeRating(String text)
        {
            String[] code = {
                ": MonoBehaviour", "UnityEngine", "System.Collections", "System.Collections.Generic",
                "void Start()", "void OnEnable()", "void OnDisable()", "void Update()", "void FixedUpdate()", 
                "new Vector2", "new Vector3", "new Vector4", "new Quaternion", 
            };

            Int32 count = text.CountOf(code);
                
            Console.WriteLine("UnityCodeCheck Count = " + count);

            return count;
        }
    }
}