// Copyright © .NET Foundation and Contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace PInvoke
{
    using System.Runtime.InteropServices;

    /// <content>
    /// Contains the <see cref="PROCESS_MITIGATION_EXTENSION_POINT_DISABLE_POLICY"/> nested type.
    /// </content>
    public partial class Kernel32
    {
        /// <summary>
        /// Contains process mitigation policy settings for legacy extension point DLLs.
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        public struct PROCESS_MITIGATION_EXTENSION_POINT_DISABLE_POLICY
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
                    /// DisableExtensionPoints : 1
                    /// ReservedFlags : 31.
                    /// </summary>
                    public uint bitvector1;

                    public bool DisableExtensionPoints
                    {
                        get => (this.bitvector1 & 1u) != 0;

                        set => this.bitvector1 = (value ? 1u : 0u) | this.bitvector1;
                    }

                    public uint ReservedFlags
                    {
                        get => (this.bitvector1 & 0xFFFFFFFEu) / 2u;

                        set => this.bitvector1 = (value * 2u) | this.bitvector1;
                    }
                }
            }
        }
    }
}
