// Copyright © .NET Foundation and Contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace PInvoke
{
    using System.Runtime.InteropServices;

    /// <content>
    /// Contains the <see cref="PROCESS_MITIGATION_IMAGE_LOAD_POLICY"/> nested type.
    /// </content>
    public partial class Kernel32
    {
        /// <summary>
        /// Contains process mitigation policy settings for the loading of images from a remote device.
        /// </summary>
        public struct PROCESS_MITIGATION_IMAGE_LOAD_POLICY
        {
            /// <summary>
            /// A union of flags and a bit-vector of policies.
            /// </summary>
            public DUMMYUNION DUMMYUNIONNAME;

            /// <summary>
            /// A bit-vector of individual policies.
            /// </summary>
            public struct DUMMYSTRUCT
            {
                /// <summary>
                /// NoRemoteImages : 1
                /// NoLowMandatoryLabelImages : 1
                /// PreferSystem32Images : 1
                /// AuditNoRemoteImages : 1
                /// AuditNoLowMandatoryLabelImages : 1
                /// ReservedFlags : 27.
                /// </summary>
                private uint bitvector;

                /// <summary>
                /// Gets or sets a value indicating whether loading images from a remote device should be
                /// prevented.
                ///
                /// Set bit-0 of the bit-vector to (0x1) to prevent the process from loading images from a remote device, such as a UNC share;
                /// otherwise leave bit-0 unset (0x0).
                /// </summary>
                public bool NoRemoteImages
                {
                    get => (this.bitvector & 1u) != 0;
                    set => this.bitvector = (value ? 1u : 0u) | this.bitvector;
                }

                /// <summary>
                /// Gets or sets a value indicating whether the process should be prevented from loading images that have
                /// a Low Mandatory Label
                ///
                /// Set bit-1 of the <see cref=" bitvector"/> to (0x1) to prevent the process from loading images that have a Low mandatory label,
                /// as written by low IL; otherwise leave bit-1 unset (0x0).
                /// </summary>
                public bool NoLowMandatoryLabelImages
                {
                    get => ((this.bitvector & 2u) / 2) != 0;
                    set => this.bitvector = ((value ? 1u : 0u) * 2) | this.bitvector;
                }

                /// <summary>
                /// Gets or sets a value indicating whether to first search for images under System32 subfolder,
                /// then in the application directory in the standard DLL search order.
                ///
                /// Set bit-2 of the <see cref="bitvector"/> to (0x1) to search for images to load in the System32 subfolder of the folder in which Windows is
                /// installed first, then in the application directory in the standard DLL search order; otherwise leave bit-2
                /// unset (0x0).
                /// </summary>
                public bool PreferSystem32Images
                {
                    get => ((this.bitvector & 4u) / 4) != 0;
                    set => this.bitvector = ((value ? 1u : 0u) * 4) | this.bitvector;
                }

                /// <summary>
                /// Gets or sets a value indicating whether....
                /// </summary>
                public bool AuditNoRemoteImages
                {
                    get => ((this.bitvector & 8u) / 8) != 0;
                    set => this.bitvector = ((value ? 1u : 0u) * 8) | this.bitvector;
                }

                /// <summary>
                /// Gets or sets <see cref="AuditNoLowMandatoryLabelImages"/>.
                /// </summary>
                public uint AuditNoLowMandatoryLabelImages
                {
                    get => (this.bitvector & 16u) / 16;
                    set => this.bitvector = (value * 16) | this.bitvector;
                }

                /// <summary>
                /// Gets or sets <see cref="ReservedFlags"/>.
                /// </summary>
                public uint ReservedFlags
                {
                    get => (this.bitvector & 0xFFFFFFE0u) / 32;
                    set => this.bitvector = (value * 32) | this.bitvector;
                }
            }

            /// <summary>
            /// A union of flags and a bit-vector.
            /// </summary>
            [StructLayout(LayoutKind.Explicit)]
            public struct DUMMYUNION
            {
                /// <summary>
                /// Reserved for system use.
                /// </summary>
                [FieldOffset(0)]
                public uint Flags;

                /// <summary>
                /// Contains a bit-vector.
                /// </summary>
                [FieldOffset(0)]
                public DUMMYSTRUCT DUMMYSTRUCTNAME;
            }
        }
    }
}
