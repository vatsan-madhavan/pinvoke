// Copyright © .NET Foundation and Contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace PInvoke
{
    using System.Runtime.InteropServices;

    /// <content>
    /// Contains the <see cref="PROCESS_MITIGATION_SYSTEM_CALL_DISABLE_POLICY"/> nested type.
    /// </content>
    public partial class Kernel32
    {
        /// <summary>
        /// Used to impose restrictions on what system calls can be invoked by a process.
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        public struct PROCESS_MITIGATION_SYSTEM_CALL_DISABLE_POLICY
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
                    /// DisallowWin32kSystemCalls : 1
                    /// AuditDisallowWin32kSystemCalls : 1
                    /// ReservedFlags : 30.
                    /// </summary>
                    public uint bitvector1;

                    public bool DisallowWin32kSystemCalls
                    {
                        get => (this.bitvector1 & 1u) != 0;

                        set => this.bitvector1 = (value ? 1u : 0u) | this.bitvector1;
                    }

                    public bool AuditDisallowWin32kSystemCalls
                    {
                        get => ((this.bitvector1 & 2) / 2) != 0;

                        set => this.bitvector1 = ((value ? 1u : 0u) * 2) | this.bitvector1;
                    }

                    public uint ReservedFlags
                    {
                        get => (this.bitvector1 & 4294967292u) / 4;

                        set => this.bitvector1 = (value * 4) | this.bitvector1;
                    }
                }
            }
        }
    }
}
