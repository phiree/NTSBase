using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Wuqi.Webdiyer;
/// <summary>
///BindPagedList 的摘要说明
/// </summary>
public class BindPagedList<T>
{
    System.Web.UI.Page CurrentPage;
    AspNetPager pager;
    NBiz.BLLBase<T> bllBase;
    GridView gv;
    private int GetPageIndex()
    {
        
        int pageIndex = 0;
        string paramPageIndex =CurrentPage.Request[pager.UrlPageIndexName];
        if (!int.TryParse(paramPageIndex, out pageIndex))
        {
            pageIndex = 0;
        }
        return pageIndex;
    }
    
	public BindPagedList(Page page,AspNetPager pager,NBiz.BLLBase<T> bllBase,GridView gv, IList<T> list)
	{
        this.CurrentPage = page;
        this.pager = pager;
        this.bllBase = bllBase;
        this.gv = gv;
		//
		//TODO: 在此处添加构造函数逻辑
		//
	}
}