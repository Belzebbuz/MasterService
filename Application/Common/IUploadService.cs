using Application.Common.Contracts;
using Shared.Messages.Identity;

namespace Application.Common;

public interface IUploadService : ITransientService
{
    string Upload(UploadRequest request);
}
