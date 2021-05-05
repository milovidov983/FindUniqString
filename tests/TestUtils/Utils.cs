using BaseAbstractions;
using System;

namespace TestUtils {
	public static class Factory {
		public static IFile CreateBasicString(string content) {
			return new BasicString.Implementation(content);
		}

		public static IFile CreateBasicFile(string path) {
			return new BasicFile.Implementation(path);
		}
	}
}
