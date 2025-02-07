using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyShop.Core.Enums
{
    public enum UserType
    {
        /// <summary>
        /// Job seeker who can apply for positions
        /// </summary>
        Candidate = 0,

        /// <summary>
        /// Human Resources professional belonging to a company
        /// </summary>
        HR = 1,

        /// <summary>
        /// Talent acquisition specialist (can be independent or company-affiliated)
        /// </summary>
        Recruiter = 2,

        /// <summary>
        /// System administrator with full privileges
        /// </summary>
        SuperAdmin = 3
    }
}
