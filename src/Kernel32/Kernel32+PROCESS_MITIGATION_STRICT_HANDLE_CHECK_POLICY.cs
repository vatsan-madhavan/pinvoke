// Copyright © .NET Foundation and Contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace PInvoke
{
    using System.Runtime.InteropServices;

    /// <content>
    /// Contains the <see cref="PROCESS_MITIGATION_STRICT_HANDLE_CHECK_POLICY"/> nested type.
    /// </content>
    public partial class Kernel32
    {
        /// <summary>
        /// Used to impose new behavior on handle references that are not valid.
        /// </summary>
        /// <remarks>
        /// As a general rule, strict handle checking cannot be turned off once it is turned on.
        /// Therefore, when calling the SetProcessMitigationPolicy function with this policy, the values of the
        /// <see cref="DUMMYSTRUCT.RaiseExceptionOnInvalidHandleReference"/> and
        /// <see cref="DUMMYSTRUCT.HandleExceptionsPermanentlyEnabled"/> substructure members must be the same. It is not possible to
        /// enable invalid handle exceptions only temporarily.
        ///
        /// The exception to the general rule about strict handle checking always being a permanent state is that debugging tools
        /// such as Application Verifier can cause the operating system to enable invalid handle exceptions temporarily.Under those
        /// cases, it is possible for the GetProcessMitigationPolicy function to return with
        /// <see cref="DUMMYSTRUCT.RaiseExceptionOnInvalidHandleReference"/>> set to true, but <see cref="DUMMYSTRUCT.HandleExceptionsPermanentlyEnabled"/>
        /// set to false.
        /// </remarks>
        public struct PROCESS_MITIGATION_STRICT_HANDLE_CHECK_POLICY
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
                /// RaiseExceptionOnInvalidHandleReference : 1
                /// HandleExceptionsPermanentlyEnabled : 1
                /// ReservedFlags : 30.
                /// </summary>
                private uint bitvector;

                public bool RaiseExceptionOnInvalidHandleReference
                {
                    get => (this.bitvector & 1u) != 0;
                    set => this.bitvector = (value ? 1u : 0u) | this.bitvector;
                }

                public bool HandleExceptionsPermanentlyEnabled
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
