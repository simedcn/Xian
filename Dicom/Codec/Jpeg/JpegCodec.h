#pragma region License (non-CC)

// Copyright (c) 2010, ClearCanvas Inc.
// All rights reserved.
//
// This library is free software; you can redistribute it and/or
// modify it under the terms of the GNU Lesser General Public
// License as published by the Free Software Foundation; either
// version 2.1 of the License, or (at your option) any later version.
//
// This library is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
// Lesser General Public License for more details.
//
// You should have received a copy of the GNU Lesser General Public
// License along with this library; if not, write to the Free Software
// Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA  02111-1307  USA

#pragma endregion

#ifndef __JPEGCODEC_H__
#define __JPEGCODEC_H__

#pragma once

using namespace System;
using namespace System::IO;
using namespace System::Threading;

using namespace ClearCanvas::Common;
using namespace ClearCanvas::Dicom::Codec;
using namespace ClearCanvas::Dicom;

#include "DicomJpegParameters.h"

namespace ClearCanvas {
namespace Dicom {
namespace Codec {
namespace Jpeg {

public enum class JpegMode : int {
	Baseline,
	Sequential,
	SpectralSelection,
	Progressive,
	Lossless
};

public ref class IJpegCodec abstract {
public:
	virtual void Encode(DicomUncompressedPixelData^ oldPixelData, DicomCompressedPixelData^ newPixelData, DicomJpegParameters^ params, int frame) abstract;
	virtual void Decode(DicomCompressedPixelData^ oldPixelData, DicomUncompressedPixelData^ newPixelData, DicomJpegParameters^ params, int frame) abstract;

internal:
	MemoryStream^ MemoryBuffer;
	array<unsigned char>^ DataBuffer;
	unsigned char* DataPtr;

	JpegMode Mode;
	int Predictor;
	int PointTransform;
};

public ref class Jpeg16Codec : public IJpegCodec {
public:
	Jpeg16Codec(JpegMode mode, int predictor, int point_transform);
	virtual void Encode(DicomUncompressedPixelData^ oldPixelData, DicomCompressedPixelData^ newPixelData, DicomJpegParameters^ params, int frame) override;
	virtual void Decode(DicomCompressedPixelData^ oldPixelData, DicomUncompressedPixelData^ newPixelData, DicomJpegParameters^ params, int frame) override;

internal:
	[ThreadStatic]
	static Jpeg16Codec^ This;
};

public ref class Jpeg12Codec : public IJpegCodec {
public:
	Jpeg12Codec(JpegMode mode, int predictor, int point_transform);
	virtual void Encode(DicomUncompressedPixelData^ oldPixelData, DicomCompressedPixelData^ newPixelData, DicomJpegParameters^ params, int frame) override;
	virtual void Decode(DicomCompressedPixelData^ oldPixelData, DicomUncompressedPixelData^ newPixelData, DicomJpegParameters^ params, int frame) override;

internal:
	[ThreadStatic]
	static Jpeg12Codec^ This;
};

public ref class Jpeg8Codec : public IJpegCodec {
public:
	Jpeg8Codec(JpegMode mode, int predictor, int point_transform);
	virtual void Encode(DicomUncompressedPixelData^ oldPixelData, DicomCompressedPixelData^ newPixelData, DicomJpegParameters^ params, int frame) override;
	virtual void Decode(DicomCompressedPixelData^ oldPixelData, DicomUncompressedPixelData^ newPixelData, DicomJpegParameters^ params, int frame) override;

internal:
	[ThreadStatic]
	static Jpeg8Codec^ This;
};

} // Jpeg
} // Codec
} // Dicom
} // ClearCanvas

#endif