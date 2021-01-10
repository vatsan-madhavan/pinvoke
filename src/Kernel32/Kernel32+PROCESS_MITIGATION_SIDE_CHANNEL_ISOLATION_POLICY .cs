// Copyright © .NET Foundation and Contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace PInvoke
{
    using System.Runtime.InteropServices;

    /// <content>
    /// Contains the <see cref="PROCESS_MITIGATION_SIDE_CHANNEL_ISOLATION_POLICY"/> nested type.
    /// </content>
    public partial class Kernel32
    {
        /// <summary>
        /// This data structure provides the status of process policies that are related to the mitigation of side channels.
        /// This can include side channel attacks involving speculative execution and page combining.
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        public struct PROCESS_MITIGATION_SIDE_CHANNEL_ISOLATION_POLICY
        {
            public DUMMYUNION DUMMYUNIONNAME;

            [StructLayout(LayoutKind.Explicit)]
            public struct DUMMYUNION
            {
                /// <summary>
                /// This member is reserved for system use.
                /// </summary>
                [FieldOffset(0)]
                public int Flags;

                [FieldOffset(0)]
                public DUMMYSTRUCT DUMMYSTRUCTNAME;

                [StructLayout(LayoutKind.Sequential)]
                public struct DUMMYSTRUCT
                {
                    /// <summary>
                    /// SmtBranchTargetIsolation : 1
                    /// IsolateSecurityDomain : 1
                    /// DisablePageCombine : 1
                    /// SpeculativeStoreBypassDisable : 1
                    /// ReservedFlags : 28.
                    /// </summary>
                    private uint bitvector;

                    /// <summary>
                    /// Gets or sets a value indicating whether prevent branch target pollution cross-SMT-thread in user mode.
                    /// </summary>
                    public bool SmtBranchTargetIsolation
                    {
                        get => (this.bitvector & 1u) != 0;

                        set => this.bitvector = (value ? 1u : 0u) | this.bitvector;
                    }

                    /// <summary>
                    /// Gets or sets a value indicating whether isolate this process into a distinct security domain, even from other processes running as the same
                    /// security context. This prevents branch target injection cross-process.
                    ///
                    /// Page combining is limited to processes within the same security domain.This flag effectively limits
                    /// the process to only combining internally to the process itself, except for common pages and unless
                    /// further restricted by the <see cref="DisablePageCombine"/> policy.
                    /// </summary>
                    public bool IsolateSecurityDomain
                    {
                        get => ((this.bitvector & 2u) / 2u) != 0;

                        set => this.bitvector = ((value ? 1u : 0u) * 2u) | this.bitvector;
                    }

                    /// <summary>
                    /// Gets or sets a value indicating whether disable all page combining for this process, even internally to the process itself,
                    /// except for common pages.
                    /// </summary>
                    public bool DisablePageCombine
                    {
                        get => ((this.bitvector & 4u) / 4u) != 0;

                        set => this.bitvector = ((value ? 1u : 0u) * 4u) | this.bitvector;
                    }

                    /// <summary>
                    /// Gets or sets a value indicating whether memory disambiguation is disabled.
                    /// </summary>
                    public bool SpeculativeStoreBypassDisable
                    {
                        get => ((this.bitvector & 8u) / 8u) != 0;

                        set => this.bitvector = ((value ? 1u : 0u) * 8u) | this.bitvector;
                    }

                    /// <summary>
                    /// Gets or sets a value that is reserved for system use.
                    /// </summary>
                    public uint ReservedFlags
                    {
                        get => (this.bitvector & 0xFFFFFFF0u) / 16u;

                        set => this.bitvector = (value * 16u) | this.bitvector;
                    }
                }
            }
        }
    }
}
