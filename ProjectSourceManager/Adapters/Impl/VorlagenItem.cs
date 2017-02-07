using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectSourceManager.Adapters.Impl
{
    public class VorlagenItem : AdaptedItem
    {
        private String _aliasedFile;


        public VorlagenItem(AdapterBase adapter, String name, ProjectDirectory project)
            : base(adapter, name, project)
        {
            _aliasedFile = String.Format(@"{0}\{1}", Project.Settings.VorlagenDir, Name);
        }

        public override void Push(byte[] data)
        {
            if (data == null && File.Exists(_aliasedFile))
            {
                File.Delete(_aliasedFile);
            }
            else
            {
                File.WriteAllBytes(_aliasedFile, data);
            }
        }

        public override byte[] Pull()
        {
            if (!File.Exists(_aliasedFile))
                return null;

            return File.ReadAllBytes(_aliasedFile);
        }
    }
}
