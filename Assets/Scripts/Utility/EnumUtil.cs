﻿using System;
using System.Collections.Generic;

public class EnumUtil
{
    public static TEnum[] GetEnumList<TEnum>() where TEnum : Enum 
        => ((TEnum[])Enum.GetValues(typeof(TEnum)));
}
