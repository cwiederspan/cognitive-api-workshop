using System;
using System.Collections.Generic;
using System.Text;

namespace ImageFunctionApp {

    public static class AppSettings {

        public static string GetEnvironmentVariable(string name) {
            return System.Environment.GetEnvironmentVariable(name, EnvironmentVariableTarget.Process);
        }
    }
}