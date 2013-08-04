using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Corex.IO
{
    public struct FsPath
    {
        public FsPath(string path)
        {
            Path.IsPathRooted(path);
            _Value = path;
        }
        public static implicit operator FsPath(string value)
        {
            return new FsPath(value);
        }
        public static implicit operator string(FsPath path)
        {
            return path._Value;
        }
        public static FsPath operator +(FsPath x, FsPath y)
        {
            return x.Child(y);
        }
        string _Value;
        public FsPath Parent
        {
            get
            {
                return new FsPath(Path.GetDirectoryName(_Value));
            }
        }
        public FsPath this[string name]
        {
            get
            {
                return Child(name);
            }
        }
        public FileInfo ToFileInfo()
        {
            return new FileInfo(_Value);
        }
        public DirectoryInfo ToDirectoryInfo()
        {
            return new DirectoryInfo(_Value);
        }
        public FsPath Child(string name)
        {
            return CombineWith(name);
        }
        public FsPath CombineWith(string name)
        {
            return new FsPath(Path.Combine(_Value, name));
        }
        public FsPath Sibling(string name)
        {
            return Parent.Child(name);
        }
        public FsPath NameWithoutExtension
        {
            get
            {
                return new FsPath(Path.GetFileNameWithoutExtension(_Value));
            }
        }
        public FsPath Name
        {
            get
            {
                return new FsPath(Path.GetFileName(_Value));
            }
        }
        public FsPath ChangeExtension(string ext)
        {
            return new FsPath(Path.ChangeExtension(_Value, ext));
        }
        public override string ToString()
        {
            return _Value;
        }
    }
}
