namespace ToolBX.FileGuy;

[AutoConfig("DefaultFileFetching")]
public sealed record FileFetchOptions
{
    public IReadOnlyList<string> FileExtensions
    {
        get => _fileExtensions;
        init => _fileExtensions = value?.ToImmutableList() ?? ImmutableList<string>.Empty;
    }
    private readonly IReadOnlyList<string> _fileExtensions = Array.Empty<string>();

    public SearchOption SearchKind
    {
        get => _searchKind;
        init => _searchKind = value.ThrowIfUndefined();
    }
    private readonly SearchOption _searchKind = SearchOption.TopDirectoryOnly;

    public UriKind UriKind
    {
        get => _uriKind;
        init => _uriKind = value.ThrowIfUndefined();
    }
    private readonly UriKind _uriKind = UriKind.Absolute;

    public bool Equals(FileFetchOptions? other) => other != null && FileExtensions.SequenceEqual(other.FileExtensions) && SearchKind == other.SearchKind && UriKind == other.UriKind;

    public override int GetHashCode()
    {
        var hashCode = new HashCode();

        foreach (var extension in FileExtensions)
        {
            hashCode.Add(extension, StringComparer.OrdinalIgnoreCase);
        }

        hashCode.Add(SearchKind);
        hashCode.Add(UriKind);

        return hashCode.ToHashCode();
    }
}