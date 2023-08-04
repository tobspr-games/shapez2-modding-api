using System;
using System.Collections.Generic;
using System.Text;
using HarmonyLib;

internal class ReplaceAttribute : Harmony
{
    public ReplaceAttribute(string id) : base(id)
    {
    }
}
