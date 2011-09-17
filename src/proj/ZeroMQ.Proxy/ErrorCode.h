#pragma once

#include <errno.h>

namespace ZeroMQ {
namespace Proxy {

    public enum class ErrorCode
    {
        Eperm = EPERM,
        Enoent = ENOENT,
        Esrch = ESRCH,
        Eintr = EINTR,
        Eio = EIO,
        Enxio = ENXIO,
        E2big = E2BIG,
        Enoexec = ENOEXEC,
        Ebadf = EBADF,
        Echild = ECHILD,
        Eagain = EAGAIN,
        Enomem = ENOMEM,
        Eacces = EACCES,
        Efault = EFAULT,
        Ebusy = EBUSY,
        Eexist = EEXIST,
        Exdev = EXDEV,
        Enodev = ENODEV,
        Enotdir = ENOTDIR,
        Eisdir = EISDIR,
        Enfile = ENFILE,
        Emfile = EMFILE,
        Enotty = ENOTTY,
        Efbig = EFBIG,
        Enospc = ENOSPC,
        Espipe = ESPIPE,
        Erofs = EROFS,
        Emlink = EMLINK,
        Epipe = EPIPE,
        Edom = EDOM,
        Edeadlk = EDEADLK,
        Enametoolong = ENAMETOOLONG,
        Enolck = ENOLCK,
        Enosys = ENOSYS,
        Enotempty = ENOTEMPTY
    };

} }