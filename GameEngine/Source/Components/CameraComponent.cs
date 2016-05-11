using Microsoft.Xna.Framework;

namespace GameEngine {
    public class CameraComponent : IComponent {
        public float nearClipPlane = 1f;
        public float farClipPlane { get; set; }
        public Matrix projectionMatrix;
        public Matrix viewMatrix;

        public float aspectRatio { get; set; }
        public float fieldOfView { get; set; }
        public Vector3 upDirection { get; set; }
        public Vector3 position { get; set; }
        public Vector3 target { get; set; }
        public string targetEntity { get; set; }    //target entity to chase / look at
        public Vector3 camChasePosition { get; set; }
        public int cameraMode { get; set; }
        public BoundingFrustum cameraFrustrum { get; set; }

        public CameraComponent(GraphicsDeviceManager graphics, Vector3 position, Vector3 direction) {
            aspectRatio = graphics.PreferredBackBufferWidth / (float)graphics.PreferredBackBufferHeight;
            fieldOfView = MathHelper.PiOver4;
            upDirection = Vector3.Up;
            viewMatrix = Matrix.CreateLookAt(position, direction, upDirection);
            farClipPlane = 500;
            Matrix.CreatePerspectiveFieldOfView(fieldOfView, aspectRatio, nearClipPlane, farClipPlane, out projectionMatrix);
            targetEntity = null;
            cameraMode = 0;
            camChasePosition = Vector3.Zero;
        }

        public CameraComponent(GraphicsDeviceManager graphics) {
            aspectRatio = graphics.PreferredBackBufferWidth / (float)graphics.PreferredBackBufferHeight;
            fieldOfView = MathHelper.PiOver4;
            upDirection = Vector3.Up;
            target = Vector3.Zero;
            position = new Vector3(10, 20, 200);
            viewMatrix = Matrix.CreateLookAt(position, target, upDirection);
            farClipPlane = 500;
            Matrix.CreatePerspectiveFieldOfView(fieldOfView, aspectRatio, nearClipPlane, farClipPlane, out projectionMatrix);
            targetEntity = null;
            cameraMode = 0;
            camChasePosition = Vector3.Zero;
        }

    }
}
