using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI.WebControls;
namespace NLibrary
{
   public class ButtonExt:Button
    {
       protected override void OnLoad(EventArgs e)
       {
           this.OnClientClick = "javascript:this.value='请稍候.....';return true;";
           base.OnLoad(e);
       }
    }
}
