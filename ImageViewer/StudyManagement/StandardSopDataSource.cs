using System;

namespace ClearCanvas.ImageViewer.StudyManagement
{
	public abstract class StandardSopDataSource : SopDataSource
	{
		//I hate doing this, but it's horribly inefficient for all subclasses to do their own locking.
		protected readonly object SyncLock = new object();
		private volatile WeakReference[] _framePixelData;

		protected StandardSopDataSource()
			: base()
		{
		}

		private WeakReference[] FramePixelData
		{
			get
			{
				if (_framePixelData == null)
				{
					lock(SyncLock)
					{
						if (_framePixelData == null)
						{
							_framePixelData = new WeakReference[NumberOfFrames];
							for (int i = 0; i < _framePixelData.Length; ++i)
								_framePixelData[i] = new WeakReference(null);
						}
					}
				}

				return _framePixelData;
			}
		}

		protected abstract byte[] CreateFrameNormalizedPixelData(int frameNumber);

		protected sealed override void OnGetFrameNormalizedPixelData(int frameNumber, out byte[] pixelData)
		{
			int frameIndex = frameNumber - 1;
			WeakReference reference = FramePixelData[frameIndex];

			try
			{
				pixelData = reference.Target as byte[];
			}
			catch(InvalidOperationException)
			{
				pixelData = null;
				reference = new WeakReference(null);
				FramePixelData[frameIndex] = reference;
			}

			if (!reference.IsAlive || pixelData == null)
			{
				lock(SyncLock)
				{
					pixelData = CreateFrameNormalizedPixelData(frameNumber);
					reference.Target = pixelData;
				}
			}
		}

		
		//NOTE: no need to implement anything here, at least for pixel data, since we're using a WeakReferenceCache.
		protected virtual void OnUnloadFrameData(int frameNumber)
		{
		}

		public sealed override void UnloadFrameData(int frameNumber)
		{
			lock(SyncLock)
			{
				OnUnloadFrameData(frameNumber);
			}
		}
	}
}
