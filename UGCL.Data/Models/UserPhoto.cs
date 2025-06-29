namespace UGCL.Data.Models;

[InternalUse]
[Index(nameof(UserId), IsUnique = true)]
public class UserPhoto
    : TrackingBase
{
    public int UserPhotoId { get; set; }

    public required string UserId { get; set; }
    public User? User { get; set; }

    public required byte[] Content { get; set; }
}