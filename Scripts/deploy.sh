#!/bin/bash
set -e

throw_error() {
        exit_code=$2
        msg=$1

        echo ""
        print_log "ERROR" "$msg"
        echo ""
        exit ${exit_code}
}

info() {
        msg=$1
        print_log "INFO " "$msg"
}

debug() {
        if [ $DEBUG = TRUE ]; then
                msg=$1
                print_log "DEBUG" "$msg"
        fi
}

print_log() {
    log_level=$1
        msg=$2
    printf "[`date +%Y-%m-%d' '%H:%M:%S`]::${log_level}::$msg\n"
}

usage() {
    if [ -z "$USAGE_MSG" ]; then
        throw_error "Usage message not defined" 1
    fi
    info "$USAGE_MSG"
    exit 0
}

usage_error() {
    if [ -z "$USAGE_MSG" ]; then
        throw_error "Usage message not defined" 1
    fi
    throw_error "$1
    $USAGE_MSG" $PARAM_ERROR
}

###########################################################################################
###########################################################################################

USAGE_MSG="Usage ([*] = optional param): $0
        Install path [-p sale/saleAggregation]
        TAR file [-t sale-aggregation*.tar.gz]
        [*]Check md5sum [-m 1735623gfs921qhd]
        [*]Backup current deploy path [-b backups/sales/]
        [*]Do not delete deployed tar.gz file? [-d]"
while getopts ":p:t:m:b:dh" o; do
    case "${o}" in
        p)INSTALL_PATH=$HOME/${OPTARG};;
        t)TAR=${OPTARG};;
        b)BACK_UP=${OPTARG};;
	m)MD5_CHECK=${OPTARG};;
        d)DO_NOT_DELETE_TAR=TRUE;;
        h)usage;;
        :)throw_error "-${OPTARG} requires an argument." $PARAM_ERROR;;
	*)usage_error "Option/Param [${OPTARG}] not recognized";;
    esac
done
shift $((OPTIND-1))


if [ -z "$INSTALL_PATH" ]; then
    echo ""
    throw_error "Mandatory param INSTALL_PATH. Use -h param to show help"
fi
if [ -z "$TAR" ]; then
    echo ""
    throw_error "Mandatory param TAR. Use -h param to show help"
fi

info "Routing to "$INSTALL_PATH
cd $INSTALL_PATH

MD5SUM=`md5sum $TAR`
echo ""
info "MD5SUM: " | md5sum $TAR
EXPECTED_MD5SUM=`echo "$MD5SUM" | awk '{ print $1 }'`
if [ ! -z "$MD5_CHECK" ]; then
    if [[ "$MD5_CHECK" != "$EXPECTED_MD5SUM" ]]; then
        throw_error "MD5 SUM VALIDITY FAILED. Expected [$MD5_CHECK] but found [$EXPECTED_MD5SUM]" 1
    fi
fi

if [ ! -z "$BACK_UP" ]; then
    echo ""
    info "Backing up current files..."
    tar -czvf $(date +%Y%m%d-%H%M%S).tar.gz.backup bin conf lib
    mv -v $(date +%Y%m%d-%H%M%S).tar.gz.backup "$BACK_UP"/
fi

echo ""
info "## DELETING bin, conf & lib folders"
rm -rf bin/ conf/ lib/

echo ""
info "## Uncompressing $TAR..."
tar -xzvf $TAR

echo ""
info "## Giving permissions"
chmod -vR +x bin/*

echo ""
info "## Extracting DES configuration"
cp -va conf/desa/* conf/

if [ -z "$DO_NOT_DELETE_TAR" ]; then
    echo ""
    info "Removing tar.gz file"
    rm -rfv $TAR
fi

echo ""
info "MD5SUM TAR: $MD5SUM"
info "DONE!"

info "Showing deployment info:"
echo "" | cat deploy_info.txt
echo ""
echo ""
