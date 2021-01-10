// Copyright © .NET Foundation and Contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace PInvoke
{
    using System.Runtime.InteropServices;

    /// <content>
    /// Contains the <see cref="PROCESS_MITIGATION_CONTROL_FLOW_GUARD_POLICY"/> nested type.
    /// </content>
    public partial class Kernel32
    {
        /// <summary>
        /// Contains process mitigation policy settings for Control Flow Guard (CFG).
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        public struct PROCESS_MITIGATION_CONTROL_FLOW_GUARD_POLICY
        {
            public DUMMYUNION DUMMYUNIONNAME;

            [StructLayout(LayoutKind.Explicit)]
            public struct DUMMYUNION
            {
                [FieldOffset(0)]
                public int Flags;

                [FieldOffset(0)]
                public DUMMYSTRUCT DUMMYSTRUCTNAME;

                [StructLayout(LayoutKind.Sequential)]
                public struct DUMMYSTRUCT
                {
                    /// <summary>
                    /// EnableControlFlowGuard : 1
                    /// EnableExportSuppression : 1
                    /// StrictMode : 1
                    /// ReservedFlags : 29.
                    /// </summary>
                    public uint bitvector;

                    public bool EnableControlFlowGuard
                    {
                        get => (this.bitvector & 1u) != 0;

                        set => this.bitvector = (value ? 1u : 0u) | this.bitvector;
                    }

                    public bool EnableExportSuppression
                    {
                        get => ((this.bitvector & 2u) / 2u) != 0;

                        set => this.bitvector = ((value ? 1u : 0u) * 2u) | this.bitvector;
                    }

                    public bool StrictMode
                    {
                        get => ((this.bitvector & 4u) / 4u) != 0;

                        set => this.bitvector = ((value ? 1u : 0u) * 4u) | this.bitvector;
                    }

                    public uint ReservedFlags
                    {
                        get => (this.bitvector & 0xFFFFFFF8u) / 8u;

                        set => this.bitvector = (value * 8u) | this.bitvector;
                    }
                }
            }
        }
    }
}
