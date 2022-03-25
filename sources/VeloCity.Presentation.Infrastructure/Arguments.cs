// Velo City
// Copyright (C) 2022 Dust in the Wind
// 
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <http://www.gnu.org/licenses/>.

using System;
using System.Collections.Generic;
using System.Linq;

namespace DustInTheWind.VeloCity.Presentation.Infrastructure
{
    public class Arguments
    {
        private readonly List<Argument> arguments = new();

        public int Count => arguments.Count;

        public Argument this[int index]
        {
            get
            {
                bool isValidIndex = index >= 0 && index < arguments.Count;

                return isValidIndex
                    ? arguments[index]
                    : null;
            }
        }

        public Argument this[string name] => arguments.FirstOrDefault(x => x.Name == name);

        public Arguments(string[] args)
        {
            if (args == null) throw new ArgumentNullException(nameof(args));

            IEnumerable<Argument> newArguments = Parse(args);
            arguments.AddRange(newArguments);
        }

        private static IEnumerable<Argument> Parse(IEnumerable<string> args)
        {
            Argument argument = null;

            foreach (Arg arg in args.Select(x => new Arg(x)))
            {
                if (arg.HasNameMarker)
                {
                    if (argument != null)
                        yield return argument;

                    argument = new Argument
                    {
                        Name = arg.Value,
                        Type = ArgumentType.Named
                    };
                }
                else
                {
                    argument ??= new Argument
                    {
                        Type = ArgumentType.Ordinal
                    };

                    argument.Value = arg.Value;

                    yield return argument;
                    argument = null;
                }
            }

            if (argument != null)
                yield return argument;
        }

        public Argument GetOrdinal(int index)
        {
            return arguments
                .Where(x => x.Type == ArgumentType.Ordinal)
                .Skip(index)
                .FirstOrDefault();
        }
    }
}