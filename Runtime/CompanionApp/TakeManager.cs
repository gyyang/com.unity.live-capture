using System;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Unity.LiveCapture.CompanionApp
{
    interface ITakeManager
    {
        void SelectTake(IShot shot, SerializableGuid guid);
        void SetTakeData(TakeDescriptor descriptor);
        void DeleteTake(SerializableGuid guid);
        void SetIterationBase(IShot shot, SerializableGuid guid);
        void ClearIterationBase(IShot shot);
        Texture2D GetAssetPreview<T>(Guid guid) where T : UnityEngine.Object;
    }

    interface IAssetPreview
    {
        Texture2D GetAssetPreview<T>(Guid guid) where T : UnityEngine.Object;
    }

    class EditorAssetPreview : IAssetPreview
    {
        public Texture2D GetAssetPreview<T>(Guid guid) where T : UnityEngine.Object
        {
            var texture = default(Texture2D);
#if UNITY_EDITOR
            var asset = AssetDatabaseUtility.LoadAssetWithGuid<T>(guid);

            if (asset != null)
            {
                texture = AssetPreview.GetAssetPreview(asset);
            }
#endif
            return texture;
        }
    }

    class TakeManager : ITakeManager
    {
        public static TakeManager Default { get; } = new TakeManager();

        IAssetPreview m_AssetPreview;

        public TakeManager() : this(new EditorAssetPreview()) {}

        public TakeManager(IAssetPreview assetPreview)
        {
            m_AssetPreview = assetPreview;
        }

        public void SelectTake(IShot shot, SerializableGuid guid)
        {
            if (shot == null)
            {
                throw new ArgumentNullException(nameof(shot));
            }

#if UNITY_EDITOR
            var take = AssetDatabaseUtility.LoadAssetWithGuid<Take>(guid);

            shot.Take = take;
#endif
        }

        public void SetTakeData(TakeDescriptor descriptor)
        {
#if UNITY_EDITOR
            var take = AssetDatabaseUtility.LoadAssetWithGuid<Take>(descriptor.Guid);

            if (take != null)
            {
                var assetName = TakeBuilder.GetAssetName(
                    descriptor.SceneNumber,
                    descriptor.ShotName,
                    descriptor.TakeNumber);

                take.name = assetName;
                take.SceneNumber = descriptor.SceneNumber;
                take.ShotName = descriptor.ShotName;
                take.TakeNumber = descriptor.TakeNumber;
                take.CreationTime = DateTime.FromBinary(descriptor.CreationTime);
                take.Description = descriptor.Description;
                take.Rating = descriptor.Rating;
                take.FrameRate = descriptor.FrameRate;

                EditorUtility.SetDirty(take);

                AssetDatabase.SaveAssets();
            }
#endif
        }

        public void DeleteTake(SerializableGuid guid)
        {
#if UNITY_EDITOR
            var path = AssetDatabase.GUIDToAssetPath(guid.ToString());
            var take = AssetDatabase.LoadAssetAtPath<Take>(path);

            if (take != null)
            {
                AssetDatabase.DeleteAsset(path);
            }
#endif
        }

        public void SetIterationBase(IShot shot, SerializableGuid guid)
        {
            if (shot == null)
            {
                throw new ArgumentNullException(nameof(shot));
            }

#if UNITY_EDITOR
            var take = AssetDatabaseUtility.LoadAssetWithGuid<Take>(guid);

            if (take != null)
            {
                shot.IterationBase = take;
            }
#endif
        }

        public void ClearIterationBase(IShot shot)
        {
            if (shot == null)
            {
                throw new ArgumentNullException(nameof(shot));
            }

            shot.IterationBase = null;
        }

        public Texture2D GetAssetPreview<T>(Guid guid) where T : UnityEngine.Object
        {
            return m_AssetPreview.GetAssetPreview<T>(guid);
        }
    }
}
