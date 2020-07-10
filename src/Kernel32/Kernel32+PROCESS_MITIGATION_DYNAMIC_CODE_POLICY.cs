// Copyright © .NET Foundation and Contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace PInvoke
{
    using System.Runtime.InteropServices;

    /// <content>
    /// Contains the <see cref="PROCESS_MITIGATION_DYNAMIC_CODE_POLICY"/> nested type.
    /// </content>
    public partial class Kernel32
    {
        /// <summary>
        /// Contains process mitigation policy settings for restricting dynamic code generation and modification.
        /// </summary>
        public struct PROCESS_MITIGATION_DYNAMIC_CODE_POLICY
        {
            public DUMMYUNION DUMMYUNIONNAME;

            [StructLayout(LayoutKind.Explicit)]
            public struct DUMMYUNION
            {
                [FieldOffset(0)]
                public uint Flags;

                [FieldOffset(0)]
                public DUMMYSTRUCT DUMMYSTRUCTNAME;
            }

            public struct DUMMYSTRUCT
            {
                /// <summary>
                /// ProhibitDynamicCode : 1
                /// AllowThreadOptOut : 1
                /// AllowRemoteDowngrade : 1
                /// AuditProhibitDynamicCode : 1
                /// ReservedFlags : 28
                /// </summary>
                private uint bitvector;

                /// <summary>
                /// Gets or sets a value indicating whether to prevent the process from generating dynamic code or modifying
                /// existing executable code
                /// </summary>
                public bool ProhibitDynamicCode
                {
                    get => (this.bitvector & 1u) != 0;
                    set => this.bitvector = (value ? 1u : 0u) | this.bitvector;
                }

                /// <summary>
                /// Gets or sets a value indicating whether to allow threads to opt out of the restrictions on dynamic code generation by calling the
                /// SetThreadInformation function with the <i>ThreadInformation</i> parameter set to <b>ThreadDynamicCodePolicy</b>
                ///
                /// You should not use the <b>AllowThreadOptOut</b> and <b>ThreadDynamicCodePolicy</b>
                /// settings together to provide strong security. These settings are only intended to enable applications
                /// to adapt their code more easily for full dynamic code restrictions.
                /// </summary>
                public bool AllowThreadOptOut
                {
                    get => ((this.bitvector & 2u) / 2) != 0;
                    set => this.bitvector = ((value ? 1u : 0u) * 2) | this.bitvector;
                }

                /// <summary>
                /// Gets or sets a value indicating whether to allow non-AppContainer processes to modify all of the dynamic code settings
                /// for the calling process, including relaxing dynamic code restrictions after they have been set.
                /// </summary>
                public bool AllowRemoteDowngrade
                {
                    get => ((this.bitvector & 4u) / 4) != 0;
                    set => this.bitvector = ((value ? 1u : 0u) * 4) | this.bitvector;
                }

                public bool AuditProhibitDynamicCode
                {
                    get => ((this.bitvector & 8u) / 8) != 0;
                    set => this.bitvector = ((value ? 1u : 0u) * 8) | this.bitvector;
                }

                /// <summary>
                /// Gets or sets <see cref="ReservedFlags"/>
                ///
                /// Reserved for system use
                /// </summary>
                public uint ReservedFlags
                {
                    get => (this.bitvector & 0xFFFFFFF0u) / 16;
                    set => this.bitvector = (value * 16) | this.bitvector;
                }
            }
        }
    }
}
