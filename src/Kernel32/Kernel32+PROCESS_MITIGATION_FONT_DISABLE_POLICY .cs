// Copyright © .NET Foundation and Contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace PInvoke
{
    using System.Runtime.InteropServices;

    /// <content>
    /// Contains the <see cref="PROCESS_MITIGATION_FONT_DISABLE_POLICY "/> nested type.
    /// </content>
    public partial class Kernel32
    {
        /// <summary>
        /// Contains process mitigation policy settings for the loading of non-system fonts
        /// </summary>
        public struct PROCESS_MITIGATION_FONT_DISABLE_POLICY
        {
            /// <summary>
            /// Union containing <see cref="DUMMYUNION.Flags"/> and a bit-vector
            /// <see cref="DUMMYUNION.DUMMYSTRUCTNAME"/>
            /// </summary>
            public DUMMYUNION DUMMYUNIONNAME;

            /// <summary>
            /// Structure containing a bit-vector that's used in the union
            /// <see cref="DUMMYUNION"/>
            /// </summary>
            public struct DUMMYSTRUCT
            {
                /// <summary>
                /// DisableNonSystemFonts : 1
                /// AuditNonSystemFontLoading : 1
                /// ReservedFlags : 30
                /// </summary>
                private uint bitvector;

                /// <summary>
                /// Gets or sets a value indicating whether the process will be prevented from loading
                /// non-system fonts.
                ///
                /// Set bit-0 of <see cref="bitvector"/> to (0x1) to prevent the process from loading
                /// non-system fonts; otherwise leave bit-0 unset (0x0).
                /// </summary>
                public bool DisableNonSystemFonts
                {
                    get => (this.bitvector & 1u) != 0;
                    set => this.bitvector = (value ? 1u : 0u) | this.bitvector;
                }

                /// <summary>
                /// Gets or sets a value indicating whether an ETW event should be logged when the process attempts to load a
                /// non-system font.
                ///
                /// Set bit-1 of <see cref="bitvector"/> to (0x1) to indicate that an Event Tracing for Windows (ETW) event should be logged
                /// when the process attempts to load a non-system font; leave bit-1 unset (0x0) to indicate that
                /// an ETW event should not be logged.
                /// </summary>
                public bool AuditNonSystemFontLoading
                {
                    get => (this.bitvector & 2u) / 2 != 0;
                    set => this.bitvector = ((value ? 1u : 0u) * 2) | this.bitvector;
                }

                /// <summary>
                /// Gets or sets <see cref="ReservedFlags"/>
                ///
                /// Reserved for system use
                /// </summary>
                public uint ReservedFlags
                {
                    get => (this.bitvector & 0xFFFFFFFCu) / 4;
                    set => this.bitvector = (value * 4) | this.bitvector;
                }
            }

            /// <summary>
            /// Union type for <see cref="DUMMYUNIONNAME"/>
            /// </summary>
            [StructLayout(LayoutKind.Explicit)]
            public struct DUMMYUNION
            {
                /// <summary>
                /// Reserved for system use
                /// </summary>
                [FieldOffset(0)]
                public uint Flags;

                /// <summary>
                /// Structure containing a bit-vector that is used in the
                /// union <see cref="DUMMYUNION"/>
                /// </summary>
                [FieldOffset(0)]
                public DUMMYSTRUCT DUMMYSTRUCTNAME;
            }
        }
    }
}
