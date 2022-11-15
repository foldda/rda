// Copyright (c) 2020 Michael Chen
// Licensed under the MIT License -
// https://github.com/foldda/rda/blob/main/LICENSE

namespace UniversalDataTransport
{
    /*
     * An object implements this interface to gurrantee its "meaningful" properties 
     * 
     * #1. can be stored into an RDA object, with placement to designated locations 
     * and with no or acceptable distortion; and 
     * 
     * #2. can be restored from an RDA object with values retrived from the designated 
     * location.
     * 
     * IRda is an "abstract data type" for data objects in compliance with Universal Data Transport
     * 
     */

    public interface IRda
    {
        /// <summary>
        /// Stores properties into the RDA.
        /// </summary>
        /// <returns>An Rda instance that carries the properties of this object.</returns>
        Rda ToRda();

        /// <summary>
        /// Populate properties with the values from the RDA
        /// </summary>
        /// <param name="rda">An Rda instance that carries the properties of an object to be restored.</param>
        IRda FromRda(Rda rda);
    }
}

