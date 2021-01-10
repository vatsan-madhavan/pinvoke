// Copyright © .NET Foundation and Contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace PInvoke
{
    using System.Runtime.InteropServices;

    /// <content>
    /// Contains the <see cref="PROCESS_MITIGATION_DEP_POLICY"/> nested type.
    /// </content>
    public partial class Kernel32
    {
        /// <summary>
        /// Contains process mitigation policy settings for data execution prevention (DEP).
        /// The GetProcessMitigationPolicy and SetProcessMitigationPolicy functions use this structure.
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        public struct PROCESS_MITIGATION_DEP_POLICY
        {
            public DUMMYUNION DUMMYUNIONNAME;

            /// <summary>
            /// DEP is permanently enabled and cannot be disabled if this field is set to TRUE.
            /// </summary>
            private byte permanent;

            /// <summary>
            /// Gets or sets a value indicating whether DEP is permanently enabled and cannot be disabled.
            /// </summary>
            public bool Permanent
            {
                get => this.permanent != 0;
                set => this.permanent = (byte)(value ? 1 : 0);
            }

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

                public struct DUMMYSTRUCT
                {
                    /// <summary>
                    /// Enable : 1
                    /// DisableAtlThunkEmulation : 1
                    /// ReservedFlags : 30.
                    /// </summary>
                    private uint bitvector;

                    public bool Enable
                    {
                        get => (this.bitvector & 1u) != 0;
                        set => this.bitvector = (value ? 1u : 0u) | this.bitvector;
                    }

                    public bool DisableAtlThunkEmulation
                    {
                        get => ((this.bitvector & 2u) / 2) != 0;
                        set => this.bitvector = ((value ? 1u : 0u) * 2) | this.bitvector;
                    }

                    public uint ReservedFlags
                    {
                        get => (this.bitvector & 0xFFFFFFFCu) / 4;
                        set => this.bitvector = (value * 4) | this.bitvector;
                    }
                }
            }
        }
    }
}
