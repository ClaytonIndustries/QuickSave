
namespace CI.QuickSave.Core.Models
{
    public class Matrix4x4
    {
        public float m00;
        public float m33;
        public float m23;
        public float m13;
        public float m03;
        public float m32;
        public float m22;
        public float m02;
        public float m12;
        public float m21;
        public float m11;
        public float m01;
        public float m30;
        public float m20;
        public float m10;
        public float m31;

        public UnityEngine.Matrix4x4 ToUnityType()
        {
            return new UnityEngine.Matrix4x4()
            {
                m00 = m00,
                m33 = m33,
                m23 = m23,
                m13 = m13,
                m03 = m03,
                m32 = m32,
                m22 = m22,
                m02 = m02,
                m12 = m12,
                m21 = m21,
                m11 = m11,
                m01 = m01,
                m30 = m30,
                m20 = m20,
                m10 = m10,
                m31 = m31,
            };
        }

        public static Matrix4x4 FromUnityType(UnityEngine.Matrix4x4 matrix4X4)
        {
            return new Matrix4x4()
            {
                m00 = matrix4X4.m00,
                m33 = matrix4X4.m33,
                m23 = matrix4X4.m23,
                m13 = matrix4X4.m13,
                m03 = matrix4X4.m03,
                m32 = matrix4X4.m32,
                m22 = matrix4X4.m22,
                m02 = matrix4X4.m02,
                m12 = matrix4X4.m12,
                m21 = matrix4X4.m21,
                m11 = matrix4X4.m11,
                m01 = matrix4X4.m01,
                m30 = matrix4X4.m30,
                m20 = matrix4X4.m20,
                m10 = matrix4X4.m10,
                m31 = matrix4X4.m31,
            };
        }
    }
}