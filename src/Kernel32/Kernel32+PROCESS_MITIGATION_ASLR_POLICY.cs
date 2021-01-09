// Copyright © .NET Foundation and Contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace PInvoke
{
    using System.Runtime.InteropServices;

    /// <content>
    /// Contains the <see cref="PROCESS_MITIGATION_ASLR_POLICY"/> nested type.
    /// </content>
    public partial class Kernel32
    {
        /// <summary>
        /// Contains process mitigation policy settings for Address Space Randomization Layout (ASLR).
        /// The GetProcessMitigationPolicy and SetProcessMitigationPolicy functions use this structure.
        /// </summary>
        public struct PROCESS_MITIGATION_ASLR_POLICY
        {
            public DUMMYUNION DUMMYUNIONNAME;

            [StructLayout(LayoutKind.Explicit)]
            public struct DUMMYUNION
            {
                /// <summary>
                /// This member is reserved for system use.
                /// </summary>
                [FieldOffset(0)]
                public uint Flags;

                [FieldOffset(0)]
                public DUMMYSTRUCT DUMMYSTRUCTNAME;
            }

            public struct DUMMYSTRUCT
            {
                /// <summary>
                /// EnableBottomUpRandomization : 1
                /// EnableForceRelocateImages : 1
                /// EnableHighEntropy : 1
                /// DisallowStrippedImages : 1
                /// ReservedFlags : 28.
                /// </summary>
                private uint bitvector;

                public bool EnableBottomUpRandomization
                {
                    get => (this.bitvector & 1u) != 0;
                    set => this.bitvector = (value ? 1u : 0u) | this.bitvector;
                }

                public bool EnableForceRelocateImages
                {
                    get => ((this.bitvector & 2u) / 2) != 0;
                    set => this.bitvector = ((value ? 1u : 0u) * 2) | this.bitvector;
                }

                public bool EnableHighEntropy
                {
                    get => ((this.bitvector & 4u) / 4) != 0;
                    set => this.bitvector = ((value ? 1u : 0u) * 4) | this.bitvector;
                }

                public bool DisallowStrippedImages
                {
                    get => ((this.bitvector & 8u) / 8) != 0;
                    set => this.bitvector = ((value ? 1u : 0u) * 8) | this.bitvector;
                }

                public uint ReservedFlags
                {
                    get => (this.bitvector & 0xFFFFFFF0u) / 16;
                    set => this.bitvector = (value * 16) | this.bitvector;
                }
            }
        }
    }
}
