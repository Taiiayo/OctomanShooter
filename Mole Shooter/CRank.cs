using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Octoman_Shooter
{
    enum Ranks
    {
        NOVICE,
        [Description("NOT BAD")]
        NOT_BAD,
        MIDMAN,
        WARRIOR,
        SAVIOR
    }
}
