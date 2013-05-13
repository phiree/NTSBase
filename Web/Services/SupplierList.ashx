<%@ WebHandler Language="C#" Class="SupplierList" %>

using System;
using System.Web;
using System.Collections.Generic;
using NBiz;
using NModel;
public class SupplierList : IHttpHandler {

    BizSupplier bizSupplier = new BizSupplier();
    public void ProcessRequest (HttpContext context) {

        var request = context.Request;
        string conditions = string.Empty;

        string suppliername = request["name_startsWith"];
        int totalRecord;
        IList<Supplier> suppliers = bizSupplier.Search(suppliername,0,15,out totalRecord );
        string json = NLibrary.JosnHelper.GetJson<IList<Supplier>>(suppliers);
        context.Response.ContentType = "application/json";
        context.Response.Write(json);
    }

 
    public bool IsReusable {
        get {
            return false;
        }
    }

}