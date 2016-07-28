﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace WebUI.App_Start
{
    public class ConditionTagHelper : TagHelper
    {
        public ConditionTagHelper(Type t) : base(t)
        {
        }

        public override HtmlString LoadHtmlString()
        {
            StringBuilder sb = new StringBuilder();

            var fields = t.GetFields();

            var attrs = fields.AsParallel().Select(p =>
            {
                var attr = p.GetCustomAttributes(typeof(FieldAttribute), false)[0] as FieldAttribute;

                var inputType = "";

                var typeStr = p.FieldType.Name.ToLower();
                switch (typeStr)
                {
                    case "int32":
                        inputType = this._numberboxString;
                        break;

                    case "string":
                        inputType = this._textboxString;
                        break;

                    case "boolean":
                        inputType = string.Format(this._radioString, attr.PropertyName);
                        break;

                    default:
                        break;
                }

                return new
                {
                    title = attr.PropertyTitle,
                    name = attr.PropertyName,
                    inputType = inputType
                };

            }).ToArray();

            var html = string.Join("", attrs.AsParallel().Select(p =>
            {
                return string.Format(this._rowHtmlString, p.title, p.name, p.inputType);
            }));

            return new HtmlString(html);
        }
    }
}