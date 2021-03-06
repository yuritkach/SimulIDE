CC=gcc
CFLAGS="-O2 -Wall -g"
CPPFLAGS="--std=gnu99 -Wall -I$PWD/sim"
AVR_INC="/usr/avr"
AVR_CPPFLAGS="${CPPFLAGS} -I${AVR_INC}/include"
SIMAVR_VERSION="tag:v1.2"
DEBUGLOG="log.txt"

# configure_file(<input> <output>
#                [COPYONLY] [ESCAPE_QUOTES] [@ONLY]
#                [NEWLINE_STYLE [UNIX|DOS|WIN32|LF|CRLF] ])

# string()

# foreach(loop_var arg1 arg2 ...)
#   COMMAND1(ARGS ...)
#   COMMAND2(ARGS ...)
#   ...
# endforeach(loop_var)

# try_compile(RESULT_VAR <bindir> <srcfile|SOURCES srcfile...>
#             [CMAKE_FLAGS <flags>...]
#             [COMPILE_DEFINITIONS <defs>...]
#             [LINK_LIBRARIES <libs>...]
#             [OUTPUT_VARIABLE <var>]
#             [COPY_FILE <fileName> [COPY_FILE_ERROR <var>]])

conf=""
decl=""
array=""
for core in cores/*.c ; do
  file=$core
  global=${core/cores\/sim_}
  global=${global/.c}
  upper=$(echo $global|tr '[a-z]' '[A-Z]')
  # echo ${CC} -E ${CFLAGS} ${AVR_CPPFLAGS} $file
  if ${CC} -E ${CFLAGS} ${AVR_CPPFLAGS} $file >>${DEBUGLOG} 2>&1 ; then
    conf+="#define CONFIG_$upper 1\n"
    # include .c
  else
    echo WARNING $file did not compile, check your avr-gcc toolchain
  fi
done ;
cat > sim_core_config.h <<EOF
// Autogenerated do not edit
#ifndef __SIM_CORE_CONFIG_H__
#define __SIM_CORE_CONFIG_H__

#define CONFIG_SIMAVR_VERSION "${SIMAVR_VERSION}"
EOF
echo -e $conf >> sim_core_config.h
cat >> sim_core_config.h <<EOF
#endif
EOF

#
# This take the config file that was generated, and create the static
# table of all available cores for name lookups, as well as a C
# config file
#  
decl=""
array=""
for core in $(grep -r avr_kind_t cores/|awk -F '[ :]' '{print $1 "=" $3;}') ; do
  file=${core/=*}
  global=${core/*=}
  upper=$global
  upper=${upper/.c}
  upper=$(echo $upper|tr '[a-z]' '[A-Z]')
  decl+="#if CONFIG_$upper\nextern avr_kind_t $global;\n#endif\n"
  array+="#if CONFIG_$upper\n\t&$global,\n#endif\n"
done ; \
cat > sim_core_decl.h <<EOF
// Autogenerated do not edit
#ifndef __SIM_CORE_DECL_H__
#define __SIM_CORE_DECL_H__

#include "sim_core_config.h"
EOF
echo -e $decl >> sim_core_decl.h
cat >> sim_core_decl.h <<EOF
extern avr_kind_t * avr_kind[];
#ifdef AVR_KIND_DECL
avr_kind_t * avr_kind[] = {
EOF
echo -e $array >> sim_core_decl.h
cat >> sim_core_decl.h <<EOF
NULL
};
#endif
#endif
EOF
