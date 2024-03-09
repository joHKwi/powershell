using System.Management.Automation;
using Microsoft.SharePoint.Client;
using Microsoft.SharePoint.Client.Taxonomy;
using PnP.PowerShell.Commands.Base.PipeBinds;

namespace PnP.PowerShell.Commands.Taxonomy
{
    [Cmdlet(VerbsCommon.Get, "PnPSwitchSourceTerm")]
    public class ReassignSourceTerm : PnPRetrievalsCmdlet<Term>
    {
        [Parameter(Mandatory = true)]
        public TaxonomyTermPipeBind Identity;

        [Parameter(Mandatory = true, ValueFromPipeline = true)]
        public TaxonomyTermSetPipeBind TermSet;

        [Parameter(Mandatory = true, ValueFromPipeline = true)]
        public TaxonomyTermGroupPipeBind TermGroup;

        [Parameter(Mandatory = true)]
        public TaxonomyTermPipeBind FromIdentity;

        [Parameter(Mandatory = true, ValueFromPipeline = true)]
        public TaxonomyTermSetPipeBind FromTermSet;

        [Parameter(Mandatory = true, ValueFromPipeline = true)]
        public TaxonomyTermGroupPipeBind FromTermGroup;

        protected override void ExecuteCmdlet()
        {
            var taxonomySession = TaxonomySession.GetTaxonomySession(ClientContext);
            TermStore termStore = taxonomySession.GetDefaultSiteCollectionTermStore();
            TermGroup termGroup = TermGroup.GetGroup(termStore);
            TermSet termSet = TermSet.GetTermSet(termGroup);

            Term reassignToTerm = Identity.GetTerm(ClientContext, termStore, termSet, false, RetrievalExpressions, false);
            Term reassignFromTerm = FromIdentity.GetTerm(ClientContext, termStore, termSet, false, RetrievalExpressions, false);

            if (reassignToTerm.Id == reassignFromTerm.Id)
            {
                if (reassignFromTerm.IsSourceTerm)
                {
                    reassignFromTerm.ReassignSourceTerm(reassignToTerm);
                    ClientContext.ExecuteQueryRetry();
                }
                else
                {
                    throw new PSArgumentException($"The term with identity '{reassignFromTerm.Name}' is not a valid source term.");
                }
            }
            else
            {
                throw new PSArgumentException("Terms don't share ID.");
            }
        }
    }
}
