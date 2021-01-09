// Copyright © .NET Foundation and Contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace PInvoke
{
    /// <summary>
    /// Contains the <see cref="PROCESS_MITIGATION_POLICY"/> nested type.
    /// </summary>
    public partial class Kernel32
    {
        /// <summary>
        /// Represents the different process mitigation policies.
        /// </summary>
        public enum PROCESS_MITIGATION_POLICY
        {
            /// <summary>
            /// The data execution prevention (DEP) policy of the process
            /// </summary>
            ProcessDEPPolicy = 0,

            /// <summary>
            /// The Address Space Layout Randomization (ASLR) policy of the process
            /// </summary>
            ProcessASLRPolicy = 1,

            /// <summary>
            /// The policy that turns off the ability of the process to generate dynamic code or modify
            /// existing executable code
            /// </summary>
            ProcessDynamicCodePolicy = 2,

            /// <summary>
            /// The process will receive a fatal error if it manipulates an invalid handle.
            /// Useful for preventing downstream problems in a process due to handle misuse
            /// </summary>
            ProcessStrictHandleCheckPolicy = 3,

            /// <summary>
            /// Disables the ability to use NTUser/GDI functions at the lowest layer
            /// </summary>
            ProcessSystemCallDisablePolicy = 4,

            /// <summary>
            /// Returns the mask of valid bits for all the mitigation options on the system. An application
            /// can set many mitigation options without querying the operating system for mitigation options by
            /// combining bitwise with the mask to exclude all non-supported bits at once.
            /// </summary>
            ProcessMitigationOptionsMask = 5,

            /// <summary>
            /// The policy that prevents some built-in third party extension points from being turned on, which
            /// prevents legacy extension point DLLs from being loaded into the process.
            /// </summary>
            ProcessExtensionPointDisablePolicy = 6,

            /// <summary>
            /// The Control Flow Guard (CFG) policy of the process
            /// </summary>
            ProcessControlFlowGuardPolicy = 7,

            /// <summary>
            /// The policy of a process that can restrict image loading to those images that are either signed by
            /// Microsoft, by the Windows Store, or by Microsoft, the Windows Store and the Windows Hardware Quality Labs (WHQL).
            /// </summary>
            ProcessSignaturePolicy = 8,

            /// <summary>
            /// The policy that turns off the ability of the process to load non-system fonts
            /// </summary>
            ProcessFontDisablePolicy = 9,

            /// <summary>
            /// The policy that turns off the ability of the process to load images from some locations, such a
            /// remote devices or files that have the low mandatory label.
            /// </summary>
            ProcessImageLoadPolicy = 10,

            ProcessSystemCallFilterPolicy = 11,

            ProcessPayloadRestrictionPolicy = 12,

            ProcessChildProcessPolicy = 13,

            /// <summary>
            /// Windows 10, version 1809 and above: The policy regarding isolation of side channels for the specified process
            /// </summary>
            ProcessSideChannelIsolationPolicy = 14,

            /// <summary>
            /// Windows 10, version 2004 and above: The policy regarding user-mode Hardware-enforced Stack Protection
            /// for the process.
            /// </summary>
            ProcessUserShadowStackPolicy = 15,

            /// <summary>
            /// Ends the enumeration
            /// </summary>
            MaxProcessMitigationPolicy = 16
        }
    }
}
