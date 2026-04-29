using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using SmartBank.Data.DTO;

namespace SmartBank.API.services
{
    public interface IApplyLoanService
    {
        Task<LoanApplyDTO?> ApplyLoanAsyn(LoanApplyDTO loan, string Email);

        Task<UploadLoanDocumentDTO?> UploadLoanDocsAsync(UploadLoanDocumentDTO docs, string Email);

        Task<LoanStatusDTO?> LoanStatusAsync (LoanStatusDTO loanApplied, string Email);
    }
}