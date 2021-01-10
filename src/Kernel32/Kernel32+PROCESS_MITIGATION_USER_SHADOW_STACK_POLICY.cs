// Copyright © .NET Foundation and Contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace PInvoke
{
    using System.Runtime.InteropServices;

    /// <content>
    /// Contains the <see cref="PROCESS_MITIGATION_USER_SHADOW_STACK_POLICY"/> nested type.
    /// </content>
    public partial class Kernel32
    {
        /// <summary>
        /// Contains process mitigation policy settings for user-mode Hardware-enforced Stack Protection (HSP).
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        public struct PROCESS_MITIGATION_USER_SHADOW_STACK_POLICY
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
                    /// EnableUserShadowStack : 1
                    /// ReservedFlags : 31.
                    /// </summary>
                    private uint bitvector;

                    /// <summary>
                    /// Gets or sets a value indicating whether the user-mode Hardware-enforced Stack Protection is
                    /// enabled for the process in compatibility mode. This means that the CPU verifies function return addresses
                    /// at runtime by employing a shadow stack mechanism, if supported by the hardware. In compatibility mode,
                    /// only shadow stack violations occurring in modules compiled with CETCOMPAT are fatal. This field cannot be
                    /// changed via SetProcessMitigationPolicy function.
                    /// </summary>
                    public bool EnableUserShadowStack
                    {
                        get => (this.bitvector & 1u) != 0;

                        set => this.bitvector = (value ? 1u : 0u) | this.bitvector;
                    }

                    /// <summary>
                    /// Gets or sets a value which is reserved for system use.
                    /// </summary>
                    public uint ReservedFlags
                    {
                        get => (this.bitvector & 0xFFFFFFFEu) / 2u;

                        set => this.bitvector = (value * 2u) | this.bitvector;
                    }
                }
            }
        }
    }
}
