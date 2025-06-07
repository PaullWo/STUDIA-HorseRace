using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BibliotekaWspolna
{
    public interface IListaKoni
    {
        public List<KonPodstawowy> zwrocKonie();
        public int WielkoscPakietu();
    }
}
