using System.ComponentModel;

namespace Shared.Messages.Identity;

public class UploadRequest
{
	public string? FileName { get; set; }
	public string? Extension { get; set; }
	public UploadType UploadType { get; set; }
	public byte[]? Data { get; set; }
}

public enum UploadType : byte
{
	[Description(@"Images\Products")]
	Product,

	[Description(@"Images\ProfilePictures")]
	ProfilePicture,

	[Description(@"Documents")]
	Document
}