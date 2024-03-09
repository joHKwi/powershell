using System.Linq;
using System.Management.Automation;
using Microsoft.SharePoint.Client;
using Microsoft.SharePoint.Client.Taxonomy;


namespace PnP.PowerShell.Commands.Taxonomy
{
    [Cmdlet(VerbsCommon.Get, "PnPReassignSourceTerm")]
    public class GetTaxonomyItem : PnPSharePointCmdlet
    {
        [Parameter(Mandatory = true, ValueFromPipeline = true, Position = 0)]
        [Alias("New Source")]
        public string NewTermPath;
        [Parameter(Mandatory = true, ValueFromPipeline = true, Position = 0)]
        [Alias("Old Source")]
        public string OldTermPath;


        protected override void ExecuteCmdlet()
        {
            var Newitem = ClientContext.Site.GetTaxonomyItemByPath(NewTermPath);
            var Olditem = ClientContext.Site.GetTaxonomyItemByPath(OldTermPath);

            if (Newitem.Id == Olditem.Id){
                if(Olditem.isSource){

            Olditem.ReassignSourceTerm(Newitem);
            ClientContext.ExecuteQueryRetry();

                } else {
                     throw new Exception("Old term is not a source term");
                    }
            } else {
                 throw new Exception("Terms dosent have same id");
            }

        }

       

    }
    
    }