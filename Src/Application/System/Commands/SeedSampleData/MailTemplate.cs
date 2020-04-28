using System;

namespace BTB.Application.System.Commands.SeedSampleData
{
    public static class MailTemplate
    {
        public static string GetTemplate()
        {
            return "<html>" + Environment.NewLine +
                "    <head>" + Environment.NewLine +
                "        <style type=\"text/css\">        " + Environment.NewLine +
                "            .clean-body {" + Environment.NewLine +
                "                margin: 0;" + Environment.NewLine +
                "                padding: 0;" + Environment.NewLine +
                "                font-family: Montserrat, Trebuchet MS, Lucida Grande, Lucida Sans Unicode, Lucida Sans, Tahoma, sans-serif;" + Environment.NewLine +
                "                -webkit-text-size-adjust: 100%;" + Environment.NewLine +
                "                background-color: #007ac9;" + Environment.NewLine +
                "            }" + Environment.NewLine +
                "            .empty-row {" + Environment.NewLine +
                "                height: 30px;" + Environment.NewLine +
                "            }" + Environment.NewLine +
                "            .middle-container {" + Environment.NewLine +
                "                margin: 0 auto;" + Environment.NewLine +
                "                width: 600px;" + Environment.NewLine +
                "            }" + Environment.NewLine +
                "            .middle-content {" + Environment.NewLine +
                "                background-color: #eaeaea;  " + Environment.NewLine +
                "                width: 100%;" + Environment.NewLine +
                "                border-top-left-radius: 13px;                " + Environment.NewLine +
                "                border-top-right-radius: 13px;" + Environment.NewLine +
                "                padding-top: 40px;" + Environment.NewLine +
                "            }" + Environment.NewLine +
                "            .intive-photo {" + Environment.NewLine +
                "                width: 252px;" + Environment.NewLine +
                "                height: 78px;    " + Environment.NewLine +
                "                display:flex;" + Environment.NewLine +
                "                margin: 0 auto;  " + Environment.NewLine +
                "            }" + Environment.NewLine +
                "            .small-title {" + Environment.NewLine +
                "                font-size: 14px;" + Environment.NewLine +
                "                line-height: 1.5;" + Environment.NewLine +
                "                text-align: center;" + Environment.NewLine +
                "                word-break: break-word;" + Environment.NewLine +
                "                mso-line-height-alt: 21px;" + Environment.NewLine +
                "                margin-top: 10px;" + Environment.NewLine +
                "                color: #555555;                " + Environment.NewLine +
                "            }" + Environment.NewLine +
                "            .big-title {" + Environment.NewLine +
                "                font-size: 18px;" + Environment.NewLine +
                "                line-height: 1.2;" + Environment.NewLine +
                "                text-align: center;" + Environment.NewLine +
                "                word-break: break-word;" + Environment.NewLine +
                "                mso-line-height-alt: 22px; margin: 0;" + Environment.NewLine +
                "                color: black;" + Environment.NewLine +
                "                margin-top: 20px;" + Environment.NewLine +
                "            }" + Environment.NewLine +
                "            .divider {" + Environment.NewLine +
                "                margin-top: 15px;" + Environment.NewLine +
                "            }" + Environment.NewLine +
                "            .message {" + Environment.NewLine +
                "                text-align: center;" + Environment.NewLine +
                "                line-height: 1.5;" + Environment.NewLine +
                "                word-break: break-word;" + Environment.NewLine +
                "                font-size: 20px;" + Environment.NewLine +
                "                mso-line-height-alt: 30px;" + Environment.NewLine +
                "                margin-top: 10px;" + Environment.NewLine +
                "                color: #555555;" + Environment.NewLine +
                "            }" + Environment.NewLine +
                "            .button-container {" + Environment.NewLine +
                "                padding-right:10px;" + Environment.NewLine +
                "                padding-bottom:20px;" + Environment.NewLine +
                "                padding-left:10px;" + Environment.NewLine +
                "            }" + Environment.NewLine +
                "            .go-to-site-button {" + Environment.NewLine +
                "                text-decoration:none;" + Environment.NewLine +
                "                display:inline-block;" + Environment.NewLine +
                "                color:#007ac9;" + Environment.NewLine +
                "                background-color:white;" + Environment.NewLine +
                "                border-radius:7px;" + Environment.NewLine +
                "                -webkit-border-radius:7px;" + Environment.NewLine +
                "                -moz-border-radius:7px;" + Environment.NewLine +
                "                width:auto;" + Environment.NewLine +
                "                border-top:2px solid #007ac9;" + Environment.NewLine +
                "                border-right:2px solid #007ac9;" + Environment.NewLine +
                "                border-bottom:2px solid #007ac9;" + Environment.NewLine +
                "                border-left:2px solid #007ac9;" + Environment.NewLine +
                "                padding-top:12px;" + Environment.NewLine +
                "                padding-bottom:12px;" + Environment.NewLine +
                "                padding-left: 8px;" + Environment.NewLine +
                "                padding-right: 8px;" + Environment.NewLine +
                "                font-family:\"Montserrat\", \"Trebuchet MS\", \"Lucida Grande\", \"Lucida Sans Unicode\", \"Lucida Sans\", Tahoma, sans-serif;" + Environment.NewLine +
                "                ext-align:center;" + Environment.NewLine +
                "                mso-border-alt:none;" + Environment.NewLine +
                "                word-break:keep-all;" + Environment.NewLine +
                "            }" + Environment.NewLine +
                "            .footer {" + Environment.NewLine +
                "                background-color: #525252;" + Environment.NewLine +
                "                text-align: center;" + Environment.NewLine +
                "                color: white;" + Environment.NewLine +
                "                height: 40px;" + Environment.NewLine +
                "                border-bottom-left-radius: 13px;" + Environment.NewLine +
                "                border-bottom-right-radius: 13px;" + Environment.NewLine +
                "                padding-top: 20px;" + Environment.NewLine +
                "                font-size: 14px;" + Environment.NewLine +
                "            }" + Environment.NewLine +
                "            a {" + Environment.NewLine +
                "                color: white;" + Environment.NewLine +
                "                text-decoration: none;" + Environment.NewLine +
                "                font-weight: bold;" + Environment.NewLine +
                "            }" + Environment.NewLine +
                "            .ii a[href] {" + Environment.NewLine +
                "                color: white;" + Environment.NewLine +
                "            }" + Environment.NewLine +
                "            a[disabled] {" + Environment.NewLine +
                "                pointer-events: none;" + Environment.NewLine +
                "            }" + Environment.NewLine +
                "        </style>" + Environment.NewLine +
                "    </head>" + Environment.NewLine +
                "    <body class=\"clean-body\">" + Environment.NewLine +
                "        <div class=\"middle-container\">" + Environment.NewLine +
                "            <div class=\"empty-row\"></div>" + Environment.NewLine +
                "            <div class=\"middle-content\">" + Environment.NewLine +
                "                <img class=\"intive-photo\" src=\"https://external-content.duckduckgo.com/iu/?u=https%3A%2F%2Fnofluffjobs.com%2Fupload%2Fcompany%2Fintive.com%2Fintive_logo_blue_20180829_1039.png&f=1&nofb=1\">" + Environment.NewLine +
                "                <p class=\"small-title\">Patronage-BTB Alert</p>" + Environment.NewLine +
                "                <p class=\"big-title\">You have got an alert!</p>" + Environment.NewLine +
                "                <!-- DIVIDER -->" + Environment.NewLine +
                "                <table class=\"divider\" role=\"presentation\" style=\"table-layout: fixed; vertical-align: top; border-spacing: 0; border-collapse: collapse; mso-table-lspace: 0pt; mso-table-rspace: 0pt; min-width: 100%; -ms-text-size-adjust: 100%; -webkit-text-size-adjust: 100%;\" valign=\"top\" width=\"100%\" cellspacing=\"0\" cellpadding=\"0\" border=\"0\">" + Environment.NewLine +
                "                    <tbody>" + Environment.NewLine +
                "                    <tr style=\"vertical-align: top;\" valign=\"top\">" + Environment.NewLine +
                "                    <td class=\"divider_inner\" style=\"word-break: break-word; vertical-align: top; min-width: 100%; -ms-text-size-adjust: 100%; -webkit-text-size-adjust: 100%; padding-top: 10px; padding-right: 10px; padding-bottom: 10px; padding-left: 10px;\" valign=\"top\">" + Environment.NewLine +
                "                    <table class=\"divider_content\" role=\"presentation\" style=\"table-layout: fixed; vertical-align: top; border-spacing: 0; border-collapse: collapse; mso-table-lspace: 0pt; mso-table-rspace: 0pt; border-top: 1px solid #BBBBBB; width: 100%;\" valign=\"top\" width=\"100%\" cellspacing=\"0\" cellpadding=\"0\" border=\"0\" align=\"center\">" + Environment.NewLine +
                "                    <tbody>" + Environment.NewLine +
                "                    <tr style=\"vertical-align: top;\" valign=\"top\">" + Environment.NewLine +
                "                    <td style=\"word-break: break-word; vertical-align: top; -ms-text-size-adjust: 100%; -webkit-text-size-adjust: 100%;\" valign=\"top\"><span></span></td>" + Environment.NewLine +
                "                    </tr>" + Environment.NewLine +
                "                    </tbody>" + Environment.NewLine +
                "                    </table>" + Environment.NewLine +
                "                    </td>" + Environment.NewLine +
                "                    </tr>" + Environment.NewLine +
                "                    </tbody>" + Environment.NewLine +
                "                </table>" + Environment.NewLine +
                "                <!-- DIVIDER -->" + Environment.NewLine +
                "                <p class=\"message\">[MESSAGE]</p>" + Environment.NewLine +
                "                <div class=\"button-container\" align=\"center\">" + Environment.NewLine +
                "                    <a href=\"[DOMAIN_URL]\" class=\"go-to-site-button\">GO TO SITE</a>" + Environment.NewLine +
                "                </div>" + Environment.NewLine +
                "            </div>" + Environment.NewLine +
                "            <div class=\"footer\">" + Environment.NewLine +
                "                Intive Patronage 2020© Dotnet" + Environment.NewLine +
                "            </div>            " + Environment.NewLine +
                "            <div class=\"empty-row\"></div>" + Environment.NewLine +
                "        </div>" + Environment.NewLine +
                "    </body>" + Environment.NewLine +
                "</html>";
        }
    }
}
