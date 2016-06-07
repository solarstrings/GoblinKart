using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameEngine.Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameEngine.Components
{
    public class ModelBoundingBoxComponent : IComponent
    {
        public List<BoundingBox> boundingBoxes { get; set; }

        public ModelBoundingBoxComponent(ModelComponent modelComp)
        {
            SetupBoundingBoxes(modelComp);
        }

        private void SetupBoundingBoxes(ModelComponent modelComp)
        {
            boundingBoxes = new List<BoundingBox>();
            Matrix[] transforms = new Matrix[modelComp.model.Bones.Count];
            modelComp.model.CopyAbsoluteBoneTransformsTo(transforms);

            foreach (ModelMesh mesh in modelComp.model.Meshes)
            {
                Matrix meshTransform = transforms[mesh.ParentBone.Index];
                boundingBoxes.Add(CreateBoundingBox(mesh, meshTransform));
            }
        }

        private BoundingBox CreateBoundingBox(ModelMesh mesh, Matrix meshTransform)
        {
            // Create initial variables to hold min and max xyz values for the mesh
            Vector3 meshMaxVal = new Vector3(float.MinValue);
            Vector3 meshMinVal = new Vector3(float.MaxValue);

            foreach (ModelMeshPart p in mesh.MeshParts)
            {
                //get the bytesize of one vertex
                int byteSize = p.VertexBuffer.VertexDeclaration.VertexStride;

                VertexPositionNormalTexture[] vertData = new VertexPositionNormalTexture[p.NumVertices];
                p.VertexBuffer.GetData(p.VertexOffset * byteSize, vertData, 0, p.NumVertices, byteSize);

                // Find the min and max "xyz" values for the current mesh part
                Vector3 vertPos = new Vector3();

                for (int i = 0; i < vertData.Length; i++)
                {
                    vertPos = vertData[i].Position;

                    //update values from the vertex
                    meshMinVal = Vector3.Min(meshMinVal, vertPos);
                    meshMaxVal = Vector3.Max(meshMaxVal, vertPos);
                }
            }

            //transform by the mesh bone matrix
            meshMinVal = Vector3.Transform(meshMinVal, meshTransform);
            meshMaxVal = Vector3.Transform(meshMaxVal, meshTransform);

            //Create the bounding box
            BoundingBox boungingBox = new BoundingBox(meshMinVal, meshMaxVal);
            return boungingBox;
        }
    }
}
