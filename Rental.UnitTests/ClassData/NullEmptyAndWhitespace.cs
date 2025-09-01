using System.Collections;

namespace Rental.UnitTests.ClassData;

public class NullEmptyAndWhitespace : IEnumerable<object[]>
{
    public IEnumerator<object[]> GetEnumerator()
    {
        yield return new object[] { null! };
        yield return new object[] { string.Empty };
        yield return new object[] { " " };
        yield return new object[] { "\t" };
        yield return new object[] { "\r\n" };
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}
